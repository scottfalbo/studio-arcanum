using Arcanum.Data;
using Arcanum.ImageBlob.Interfaces;
using Arcanum.Spells;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Arcanum.Models.Interfaces.Services
{
    public class SiteService : ISite
    {
        private readonly ArcanumDbContext _db;
        public IArtistAdmin _artistAdmin;
        public IUpload _upload;

        public SiteService(ArcanumDbContext context, IArtistAdmin artistAdmin, IUpload upload)
        {
            _db = context;
            _artistAdmin = artistAdmin;
            _upload = upload;
        }

        /// <summary>
        /// Create a new artist record in the database with the user registration data.
        /// </summary>
        /// <param name="artist"> Artist object </param>
        public async Task<Artist> CreateArtist(Artist artist)
        {
            var artists = await GetArtists();
            int order = artists.Count();
            Artist newArtist = new Artist()
            {
                Id = artist.Id,
                Name = artist.Name,
                Email = artist.Email,
                ProfileImageUri = "https://via.placeholder.com/200x300",
                Display = false,
                Order = order + 1
            };

            _db.Entry(newArtist).State = EntityState.Added;
            await _db.SaveChangesAsync();
            newArtist = BootStrapAccordionIds.ArtistAccordionIds(newArtist);
            _db.Entry(newArtist).State = EntityState.Modified;
            await _db.SaveChangesAsync();

            Booking booking = await CreateBooking();
            await AddBookingToArtist(newArtist.Id, booking.Id);
            return newArtist;
        }

        /// <summary>
        /// Query an Artist record from the database by id.
        /// </summary>
        /// <param name="id"> string Artist.Id </param>
        /// <returns> Artist object </returns>
        public async Task<Artist> GetArtist(string id)
        {
            return await _db.Artist
                .Where(x => x.Id == id)
                .Include(a => a.ArtistPortfolios)
                .ThenInclude(b => b.Portfolio)
                .ThenInclude(c => c.PortfolioImage)
                .ThenInclude(d => d.Image)
                .Include(e => e.ArtistBooking)
                .ThenInclude(f => f.Booking)
                .Select(g => new Artist
                {
                    Id = g.Id,
                    Name = g.Name,
                    Email = g.Email,
                    Intro = g.Intro,
                    Instagram = g.Instagram,
                    ProfileImageUri = g.ProfileImageUri,
                    ProfileImageFileName = g.ProfileImageFileName,
                    Display = g.Display,
                    Order = g.Order,
                    ArtistPortfolios = g.ArtistPortfolios,
                    ArtistBooking = g.ArtistBooking
                }).FirstOrDefaultAsync();
        }

        /// <summary>
        /// Query a List<Artist> of all Artist records in the database.
        /// </summary>
        /// <returns> List<Artist> </returns>
        public async Task<List<Artist>> GetArtists()
        {
            return await _db.Artist
                .Include(a => a.ArtistPortfolios)
                .ThenInclude(b => b.Portfolio)
                .ThenInclude(c => c.PortfolioImage)
                .ThenInclude(d => d.Image)
                .Include(e => e.ArtistBooking)
                .ThenInclude(f => f.Booking)
                .Select(g => new Artist
                {
                    Id = g.Id,
                    Name = g.Name,
                    Email = g.Email,
                    Intro = g.Intro,
                    Instagram = g.Instagram,
                    Display = g.Display,
                    Order = g.Order,
                    ProfileImageUri = g.ProfileImageUri,
                    ProfileImageFileName = g.ProfileImageFileName,
                    ArtistPortfolios = g.ArtistPortfolios,
                    ArtistBooking = g.ArtistBooking
                }).ToListAsync();
        }

        /// <summary>
        /// Update an artist record.
        /// </summary>
        /// <param name="artist"> Artist object </param>
        public async Task UpdateArtist(Artist artist)
        {
            artist = BootStrapAccordionIds.ArtistAccordionIds(artist);
            _db.Entry(artist).State = EntityState.Modified;
            await _db.SaveChangesAsync();
        }

        /// <summary>
        /// Delete an Artist record by id.
        /// </summary>
        /// <param name="id"> string Artist.Id </param>
        public async Task DeleteArtist(string id)
        {
            Artist artist = await _artistAdmin.GetArtist(id);
            foreach(ArtistPortfolio artistPortfolio in artist.ArtistPortfolios)
            {
                await _artistAdmin.RemovePortfolioFromArtist(artistPortfolio.PortfolioId, id);
            }

            await RemoveBookingFromArtist(id, artist.ArtistBooking.BookingId);
            await DeleteBooking(artist.ArtistBooking.BookingId);
            _db.Entry(artist).State = EntityState.Deleted;
            await _db.SaveChangesAsync();
            await ReOrderArtists(artist.Order);
        }

        /// <summary>
        /// Creates a new booking record.
        /// </summary>
        /// <returns> new Booking object </returns>
        public async Task<Booking> CreateBooking()
        {
            Booking booking = new Booking();
            _db.Entry(booking).State = EntityState.Added;
            await _db.SaveChangesAsync();
            return booking;
        }

        /// <summary>
        /// Create ArtistBooking join table.
        /// </summary>
        /// <param name="artistId"> string artist id </param>
        /// <param name="bookingId"> int booking id </param>
        public async Task AddBookingToArtist(string artistId, int bookingId)
        {
            ArtistBooking artistBooking = new ArtistBooking
            {
                ArtistId = artistId,
                BookingId = bookingId
            };
            _db.Entry(artistBooking).State = EntityState.Added;
            await _db.SaveChangesAsync();
        }

        /// <summary>
        /// Updates a booking record.
        /// </summary>
        /// <param name="booking"> Booking object </param>
        public async Task UpdateBooking(Booking booking)
        {
            _db.Entry(booking).State = EntityState.Modified;
            await _db.SaveChangesAsync();
        }

        /// <summary>
        /// Removes a booking record from the database.
        /// </summary>
        /// <param name="id"> int booking id </param>
        public async Task DeleteBooking(int id)
        {
            Booking booking = await _db.Booking.FindAsync(id);
            _db.Entry(booking).State = EntityState.Deleted;
            await _db.SaveChangesAsync();
        }

        /// <summary>
        /// Removes a ArtistBooking join table record.
        /// </summary>
        /// <param name="artistId"> string artist id </param>
        /// <param name="bookingId"> int booking id </param>
        public async Task RemoveBookingFromArtist(string artistId, int bookingId)
        {
            ArtistBooking artistBooking = await _db.ArtistBooking
                .Where(x => x.ArtistId == artistId && x.BookingId == bookingId)
                .Select(y => new ArtistBooking
                {
                    ArtistId = y.ArtistId,
                    BookingId = y.BookingId
                }).FirstOrDefaultAsync();
            _db.Entry(artistBooking).State = EntityState.Deleted;
            await _db.SaveChangesAsync();
        }

        /// <summary>
        /// Gets the mainpage record.
        /// </summary>
        /// <returns> MainPage object </returns>
        public async Task<ArcanumMain> GetMainPage()
        {
            return await _db.ArcanumMain
                .Include(a => a.RecentImage)
                .ThenInclude(b => b.Image)
                .Include(c => c.PageImage)
                .ThenInclude(d => d.Image)
                .Where(x => x.Id == -1)
                .Select(y => new ArcanumMain
                {
                    Id = y.Id,
                    SiteTitle = y.SiteTitle,
                    IntroA = y.IntroA,
                    IntroB = y.IntroB,
                    RecentImage = y.RecentImage,
                    PageImage = y.PageImage
                }).FirstOrDefaultAsync();
        }

        /// <summary>
        /// Update main page record in the database.
        /// </summary>
        /// <param name="mainPage"> ArcanumMain object </param>
        public async Task UpdateMainPage(ArcanumMain mainPage)
        {
            _db.Entry(mainPage).State = EntityState.Modified;
            await _db.SaveChangesAsync();
        }

        /// <summary>
        /// Update the main page image
        /// </summary>
        /// <param name="file"> inputted file </param>
        /// <returns> newly created and uploaded image </returns>
        public async Task<Image> UpdateMainPageImage(IFormFile file)
        {
            Image image = await _upload.AddImage(file);
            Image newImage = new Image()
            {
                Title = "site-image",
                ArtistId = "site-image",
                SourceUrl = image.SourceUrl,
                ThumbnailUrl = image.ThumbnailUrl,
                FileName = image.FileName,
                ThumbFileName = image.ThumbFileName,
                Display = true
            };
            _db.Entry(newImage).State = EntityState.Added;
            await _db.SaveChangesAsync();
            return newImage;
        }

        /// <summary>
        /// Add an image to the main page model with a PageImage join table.
        /// </summary>
        /// <param name="arcanumMainId"> page id </param>
        /// <param name="imageId"> image id </param>
        public async Task AddImageToMainPage(int arcanumMainId, int imageId, int index)
        {
            PageImage pageImage = new PageImage()
            {
                ArcanumMainId = arcanumMainId,
                ImageId = imageId,
                Order = index
            };
            _db.Entry(pageImage).State = EntityState.Added;
            await _db.SaveChangesAsync();
        }

        /// <summary>
        /// Remove an main page image join table
        /// </summary>
        /// <param name="arcanumMainId"> int arcnaumMainId </param>
        /// <param name="ImageId"> int imageId </param>
        public async Task RemoveImageFromMainPage(int arcanumMainId, int imageId)
        {
            PageImage pageImage = await _db.PageImage
                .Where(x => x.ArcanumMainId == arcanumMainId && x.ImageId == imageId)
                .Select(y => new PageImage
                {
                    ArcanumMainId = y.ArcanumMainId,
                    ImageId = y.ImageId
                }).FirstOrDefaultAsync();
            _db.Entry(pageImage).State = EntityState.Deleted;
            await _db.SaveChangesAsync();
        }

        /// <summary>
        /// Query the StudioInfo record from the database.
        /// </summary>
        /// <returns> StudioInfo object </returns>
        public async Task<StudioInfo> GetStudio()
        {
            return await _db.StudioInfo
                .Where(x => x.Id == -1)
                .Include(a => a.StudioImages)
                .ThenInclude(b => b.Image)
                .Include(c => c.Address)
                .Select(y => new StudioInfo
                {
                    Id = y.Id,
                    Instagram = y.Instagram,
                    Email = y.Email,
                    Address = y.Address,
                    Intro = y.Intro,
                    Policies = y.Policies,
                    Aftercare = y.Aftercare,
                    ImageCount = y.ImageCount,
                    StudioImages = y.StudioImages
                }).FirstOrDefaultAsync();
        }

        /// <summary>
        /// Update the StudioInfo record in the database.
        /// </summary>
        /// <param name="studioInfo"> StudioInfo studioInfo </param>
        public async Task UpdateStudioInfo(StudioInfo studioInfo)
        {
            _db.Entry(studioInfo).State = EntityState.Modified;
            await _db.SaveChangesAsync();
        }

        /// <summary>
        /// Update studio address record.
        /// </summary>
        /// <param name="address"> updated address object </param>
        public async Task UpdateStudioAddress(Address address)
        {
            _db.Entry(address).State = EntityState.Modified;
            await _db.SaveChangesAsync();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public async Task<Image> AddStudioImages(IFormFile file)
        {
            Image image = await _upload.AddImage(file);
            Image newImage = new Image()
            {
                Title = "studio image",
                ArtistId = "site-image",
                SourceUrl = image.SourceUrl,
                ThumbnailUrl = image.ThumbnailUrl,
                FileName = image.FileName,
                ThumbFileName = image.ThumbFileName,
                Display = true
            };
            _db.Entry(newImage).State = EntityState.Added;
            await _db.SaveChangesAsync();
            return newImage;
        }

        /// <summary>
        /// Get a list of all studio images by join table.
        /// </summary>
        /// <param name="studioId"> int studioId </param>
        /// <returns> IEnumerable of images </returns>
        public async Task<IEnumerable<StudioImage>> GetStudioImages(int studioId)
        {
            return await _db.StudioImage
                .Where(a => a.StudioInfoId == studioId)
                .Select(x => new StudioImage
                {
                    StudioInfoId = x.StudioInfoId,
                    ImageId = x.ImageId,
                    Order = x.Order
                }).ToListAsync();
        }

        /// <summary>
        /// Create a StudioImage join table.
        /// </summary>
        /// <param name="studioId"> int studio id </param>
        /// <param name="imageId"> int image id </param>
        public async Task AddImageToStudio(int studioId, int imageId)
        {
            var studio = await GetStudio();
            StudioImage studioImage = new StudioImage()
            {
                StudioInfoId = studioId,
                ImageId = imageId,
                Order = studio.ImageCount + 1
            };
            _db.Entry(studioImage).State = EntityState.Added;
            await _db.SaveChangesAsync();
            await StudioImageCounter(-1, true);
        }

        /// <summary>
        /// Removes a StudioImage join table.
        /// </summary>
        /// <param name="studioId"> int studio id </param>
        /// <param name="imageId"> int image id </param>
        public async Task RemoveImageFromStudio(int studioId, int imageId)
        {
            StudioImage studioImage = await _db.StudioImage
                .Where(x => x.StudioInfoId == studioId && x.ImageId == imageId)
                .Select(y => new StudioImage
                {
                    StudioInfoId = y.StudioInfoId,
                    ImageId = y.ImageId,
                    Order = y.Order
                }).FirstOrDefaultAsync();
            _db.Entry(studioImage).State = EntityState.Deleted;
            await _db.SaveChangesAsync();
            await ReOrderStudioImages(-1, studioImage.Order);
            await StudioImageCounter(-1, false);
        }

        public async Task UpdateStudioImageOrder(int studioId, int order)
        {
            StudioImage image = await _db.StudioImage
                .Where(x => x.StudioInfoId == studioId)
                .Select(y => new StudioImage
                {
                    ImageId = y.ImageId,
                    StudioInfoId = y.StudioInfoId,
                    Order = order
                }).FirstOrDefaultAsync();
            _db.Entry(image).State = EntityState.Modified;
            await _db.SaveChangesAsync();
        }

        /// <summary>
        /// Increment or decrement the studio image counter.
        /// </summary>
        /// <param name="studioId"> studio id </param>
        /// <param name="increment"> bool +/- </param>
        private async Task StudioImageCounter(int studioId, bool increment)
        {
            StudioInfo studio = await _db.StudioInfo.FindAsync(studioId);
            studio.ImageCount = increment ? studio.ImageCount + 1 : studio.ImageCount - 1;
            _db.Entry(studio).State = EntityState.Modified;
            await _db.SaveChangesAsync();
        }

        /// <summary>
        /// Helper method to re-order the artists when one is removed.
        /// </summary>
        /// <param name="n"> place removed </param>
        private async Task ReOrderArtists(int n)
        {
            IEnumerable<Artist> artists = await GetArtists();
            foreach (Artist artist in artists)
            {
                if (artist.Order > n)
                {
                    artist.Order--;
                }
                await UpdateArtist(artist);
            }
        }

        /// <summary>
        /// Reorder studio images when one is removed.
        /// </summary>
        /// <param name="studioId"> studio id </param>
        /// <param name="n"> order # removed </param>
        private async Task ReOrderStudioImages(int studioId, int n)
        {
            IEnumerable<StudioImage> images = await GetStudioImages(studioId);
            foreach (StudioImage image in images)
            {
                if (image.Order > n)
                {
                    image.Order--;
                    _db.Entry(image).State = EntityState.Modified;
                    await _db.SaveChangesAsync();
                }
            }
        }
    }
}
