using Arcanum.Data;
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

        public SiteService(ArcanumDbContext context, IArtistAdmin artistAdmin)
        {
            _db = context;
            _artistAdmin = artistAdmin;
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
                .Where(x => x.Id == -1)
                .Select(y => new ArcanumMain
                {
                    Id = y.Id,
                    SiteTitle = y.SiteTitle,
                    IntroA = y.IntroA,
                    IntroB = y.IntroB,
                    RecentImage = y.RecentImage
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
        /// Query the StudioInfo record from the database.
        /// </summary>
        /// <returns> StudioInfo object </returns>
        public async Task<StudioInfo> GetStudio()
        {
            return await _db.StudioInfo
                .Where(x => x.Id == -1)
                .Include(a => a.StudioImages)
                .ThenInclude(b => b.Image)
                .Select(y => new StudioInfo
                {
                    Id = y.Id,
                    Instagram = y.Instagram,
                    Address = y.Address,
                    Intro = y.Intro,
                    Policies = y.Policies,
                    Aftercare = y.Aftercare,
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
    }
}
