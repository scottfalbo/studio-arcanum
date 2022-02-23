using Arcanum.Data;
using Arcanum.ImageBlob.Interfaces;
using Arcanum.Spells;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Arcanum.Models.Interfaces.Services
{
    public class ArtistAdminService : IArtistAdmin
    {
        private readonly ArcanumDbContext _db;
        public IUpload _upload;

        public ArtistAdminService(ArcanumDbContext db, IUpload upload)
        {
            _db = db;
            _upload = upload;
        }

        /// <summary>
        /// Create a new portfolio object and enter a record into the database.
        /// </summary>
        /// <param name="title"> string title </param>
        /// <returns> new Portfolio object </returns>
        public async Task<Portfolio> CreatePortfolio(string title)
        {
            Portfolio newPortfolio = new Portfolio()
            {
                Title = title,
                Intro = "new portfolio",
                ImageCount = 0,
                Display = false
            };

            _db.Entry(newPortfolio).State = EntityState.Added;
            await _db.SaveChangesAsync();
            newPortfolio = BootStrapAccordionIds.PortfolioAccordionIds(newPortfolio);
            _db.Entry(newPortfolio).State = EntityState.Modified;
            await _db.SaveChangesAsync();
            return newPortfolio;
        }

        public async Task<List<Portfolio>> GetPortfolios()
        {
            return await _db.Portfolio
                .Include(a => a.PortfolioImage)
                .ThenInclude(b => b.Image)
                .Select(x => new Portfolio
                {
                    Id = x.Id,
                    Title = x.Title,
                    Intro = x.Intro,
                    Display = x.Display,
                    ImageCount = x.ImageCount,
                    AccordionId = x.AccordionId,
                    CollapseId = x.CollapseId,
                    AdminAccordionId = x.AdminAccordionId,
                    AdminCollapseId = x.AdminCollapseId,
                    PortfolioImage = x.PortfolioImage
                }).ToListAsync();    
        }

        /// <summary>
        /// Update the portfolio record in the database.
        /// Reassigns accordion class names incase the portfolio title changed.
        /// </summary>
        /// <param name="portfolio"> Portfolio object </param>
        public async Task UpdatePortfolio(Portfolio portfolio)
        {
            portfolio = BootStrapAccordionIds.PortfolioAccordionIds(portfolio);
            _db.Entry(portfolio).State = EntityState.Modified;
            await _db.SaveChangesAsync();
        }

        /// <summary>
        /// Delete the portfolio record.
        /// Remove the ArtistPortfolio and associated PortfolioImage join tables.
        /// </summary>
        /// <param name="portfolioId"> int portfolioId </param>
        /// <param name="artistId"> string artistId </param>
        public async Task DeletePortfolio(int portfolioId, string artistId)
        {
            IEnumerable<PortfolioImage> images = await GetPortfolioImages(portfolioId);
            foreach (PortfolioImage image in images)
            {
                await RemoveImageFromPortfolio(portfolioId, image.Image.Id);
                await DeleteImage(image.Image.Id);
            }
            await RemovePortfolioFromArtist(portfolioId, artistId);
            Portfolio portfolio = await _db.Portfolio.FindAsync(portfolioId);
            _db.Entry(portfolio).State = EntityState.Deleted;
            await _db.SaveChangesAsync();
        }

        /// <summary>
        /// Add a portfolio object to an artist with a PortfolioArtist join table.
        /// </summary>
        /// <param name="artistId"> string artistId </param>
        /// <param name="portfolioId"> int portfolioId </param>
        public async Task AddPortfolioToArtist(string artistId, int portfolioId)
        {
            var portoflios = await GetArtistPortfolios(artistId);
            int order = portoflios.Count();

            ArtistPortfolio artistPortfolio = new ArtistPortfolio()
            {
                ArtistId = artistId,
                PortfolioId = portfolioId,
                Order = order + 1
            };
            _db.Entry(artistPortfolio).State = EntityState.Added;
            await _db.SaveChangesAsync();
        }

        /// <summary>
        /// Remove ArtistPortfolio join table record.
        /// </summary>
        /// <param name="portfolioId"> int portfolioId </param>
        /// <param name="artistId"> string artistId </param>
        public async Task RemovePortfolioFromArtist(int portfolioId, string artistId)
        {
            ArtistPortfolio artistPortfolio = await _db.ArtistPortfolio
                .Where(x => x.PortfolioId == portfolioId && x.ArtistId == artistId)
                .Select(y => new ArtistPortfolio
                {
                    ArtistId = y.ArtistId,
                    PortfolioId = y.PortfolioId,
                    Order = y.Order
                }).FirstOrDefaultAsync();
            _db.Entry(artistPortfolio).State = EntityState.Deleted;
            await _db.SaveChangesAsync();
            await ReOrderPortfolios(artistId, artistPortfolio.Order);
        }

        public async Task<List<ArtistPortfolio>> GetArtistPortfolios(string artistId)
        {
            return await _db.ArtistPortfolio
                .Where(x => x.ArtistId == artistId)
                .Select(y => new ArtistPortfolio
                {
                    ArtistId = y.ArtistId,
                    PortfolioId = y.PortfolioId,
                    Order = y.Order
                }).ToListAsync();
        }

        /// <summary>
        /// Helper method to retrieve a portfolio record and return a list of it's relational images.
        /// </summary>
        /// <param name="id"> portfolio id </param>
        /// <returns> List<Image> </returns>
        public async Task<IEnumerable<PortfolioImage>> GetPortfolioImages(int id)
        {
            return await _db.PortfolioImage
                .Where(a => a.PortfolioId == id)
                .Include(b => b.Image)
                .Select(x => new PortfolioImage
                {
                    PortfolioId = x.PortfolioId,
                    ImageId = x.ImageId,
                    Image = x.Image
                }).ToListAsync();
        }

        /// <summary>
        /// Create a new image object and record in the database.
        /// </summary>
        /// <param name="image"> Image image </param>
        /// <returns> new Image object </returns>
        public async Task<Image> CreateImage(IFormFile file, string artistId)
        {
            Artist artist = await GetArtist(artistId);
            Image image = await _upload.AddImage(file);
            Image newImage = new Image()
            {
                Title = $"Artwork by {artist.Name}",
                AltText = $"Artwork by {artist.Name}",
                ArtistId = artistId,
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
        /// Get an image from the database by id.
        /// </summary>
        /// <param name="imageId"> int imageId </param>
        /// <returns> Image object </returns>
        public async Task<Image> GetImage(int imageId)
        {
            return await _db.Image
                .Where(x => x.Id == imageId)
                .Select(y => new Image
                {
                    Id = y.Id,
                    Title = y.Title,
                    AltText = y.AltText,
                    ArtistId = y.ArtistId,
                    SourceUrl = y.SourceUrl,
                    ThumbnailUrl = y.ThumbnailUrl,
                    FileName = y.FileName,
                    ThumbFileName = y.ThumbFileName,
                    Display = y.Display
                }).FirstOrDefaultAsync();
        }

        /// <summary>
        /// Updates an image record in the database.
        /// </summary>
        /// <param name="image"> Image object </param>
        public async Task UpdateImage(Image image)
        {
            _db.Entry(image).State = EntityState.Modified;
            await _db.SaveChangesAsync();
        }

        /// <summary>
        /// Updates the order on portfolio iamge composite table.
        /// </summary>
        /// <param name="imageId"> image id </param>
        /// <param name="order"> new order </param>
        public async Task UpdateImageOrder(int imageId, int order)
        {
            PortfolioImage image = await _db.PortfolioImage
                .Where(x => x.ImageId == imageId)
                .Select(y => new PortfolioImage
                {
                    ImageId = y.ImageId,
                    PortfolioId = y.PortfolioId,
                    Order = order
                }).FirstOrDefaultAsync();
            _db.Entry(image).State = EntityState.Modified;
            await _db.SaveChangesAsync();
        }

        /// <summary>
        /// Add an image to a portfolio with a PortfolioImage join table record.
        /// </summary>
        /// <param name="portfolioId"> int portfolio id </param>
        /// <param name="imageId"> int image id </param>
        public async Task AddImageToPortfolio(int portfolioId, int imageId)
        {
            var portfolio = await _db.Portfolio.FindAsync(portfolioId);
            int order = portfolio.ImageCount;
            PortfolioImage portfolioImage = new PortfolioImage()
            {
                PortfolioId = portfolioId,
                ImageId = imageId,
                Order = order + 1
            };
            _db.Entry(portfolioImage).State = EntityState.Added;
            await _db.SaveChangesAsync();
            await PortfolioImageCounter(portfolioId, true);
        }

        /// <summary>
        /// Remove PortfolioImage join tables records.
        /// </summary>
        /// <param name="portfolioId"> int portfolioId </param>
        /// <param name="imageId"> int imageId </param>
        public async Task RemoveImageFromPortfolio(int portfolioId, int imageId)
        {
            PortfolioImage portfolioImage = await _db.PortfolioImage
                .Where(x => x.ImageId == imageId && x.PortfolioId == portfolioId)
                .Select(y => new PortfolioImage
                {
                    PortfolioId = y.PortfolioId,
                    ImageId = y.ImageId,
                    Order = y.Order
                }).FirstOrDefaultAsync();
            _db.Entry(portfolioImage).State = EntityState.Deleted;
            await _db.SaveChangesAsync();
            await ReOrderPortfolioImages(portfolioId, portfolioImage.Order);
            await PortfolioImageCounter(portfolioId, false);
        }

        /// <summary>
        /// Increment or decrement the portfolio image count based on input bool.
        /// </summary>
        /// <param name="portfolioId"> portfolio id </param>
        /// <param name="increment"> + / - </param>
        private async Task PortfolioImageCounter(int portfolioId, bool increment)
        {
            Portfolio portfolio = await _db.Portfolio.FindAsync(portfolioId);
            portfolio.ImageCount = increment ? portfolio.ImageCount++ : portfolio.ImageCount--;
            _db.Entry(portfolio).State = EntityState.Modified;
            await _db.SaveChangesAsync();
        }

        /// <summary>
        /// Creates a RecentImage join table to put recently uploaded content on the main page.
        /// If there are more than 10 images remove it removes the oldest.
        /// </summary>
        /// <param name="arcanumMainId"> ing mainPageId </param>
        /// <param name="imageId"> int imageId </param>
        public async Task AddImageToRecent(int arcanumMainId, int imageId)
        {
            var images = await GetRecentImages(arcanumMainId);
            if (images.Count() > 9)
            {
                RecentImage oldestImage = images.OrderBy(x => x.DateTime).First();
                await RemoveImageFromRecent(oldestImage.ArcanumMainId, oldestImage.ImageId);
            }

            RecentImage recentImage = new RecentImage()
            {
                ArcanumMainId = arcanumMainId,
                ImageId = imageId,
                DateTime = DateTime.Now
            };
            _db.Entry(recentImage).State = EntityState.Added;
            await _db.SaveChangesAsync();
        }

        /// <summary>
        /// Remove the RecentImage join table effectively removing the image from the main page.
        /// </summary>
        /// <param name="arcanumMainId"> int mainPageId </param>
        /// <param name="imageId"> int imageId </param>
        public async Task RemoveImageFromRecent(int arcanumMainId, int imageId)
        {
            RecentImage recentImage = await _db.RecentImage
                .Where(x => x.ArcanumMainId == arcanumMainId && x.ImageId == imageId)
                .Select(y => new RecentImage
                {
                    ArcanumMainId = y.ArcanumMainId,
                    ImageId = y.ImageId
                }).FirstOrDefaultAsync();
            if (recentImage != null)
            {
                _db.Entry(recentImage).State = EntityState.Deleted;
                await _db.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Gets a list of RecentImages by page id.
        /// </summary>
        /// <param name="arcanumMainId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<RecentImage>> GetRecentImages(int arcanumMainId)
        {
            return await _db.RecentImage
                .Where(a => a.ArcanumMainId == arcanumMainId)
                .Include(b => b.Image)
                .Select(x => new RecentImage
                {
                    ArcanumMainId = x.ArcanumMainId,
                    ImageId = x.ImageId,
                    Image = x.Image
                }).ToListAsync();
        }

        /// <summary>
        /// Removes image record, portfolio join table and files in storage blob.
        /// </summary>
        /// <param name="imageId"> int image id </param>
        /// <param name="portfolioId"> int portfolio id </param>
        /// <returns></returns>
        public async Task DeleteImage(int imageId)
        {
            Image image = await _db.Image.FindAsync(imageId);
            await _upload.RemoveImage(image.FileName);
            _db.Entry(image).State = EntityState.Deleted;
            await _db.SaveChangesAsync();
        }

        /// <summary>
        /// Query the database for an artists associated booking record.
        /// </summary>
        /// <param name="artistId"> string artistId </param>
        /// <returns> Booking object </returns>
        public async Task<Booking> GetArtistBooking(string artistId)
        {
            return await _db.ArtistBooking
                .Where(x => x.ArtistId == artistId)
                .Include(y => y.Booking)
                .Select(z => new Booking
                {
                    Id = z.Booking.Id,
                    BookingEmail = z.Booking.BookingEmail,
                    BookingInfo = z.Booking.BookingInfo,
                    FormPlaceHolder = z.Booking.FormPlaceHolder
                }).FirstOrDefaultAsync();
        }

        /// <summary>
        /// Retrieve an artist record by id.
        /// </summary>
        /// <param name="id"> string id </param>
        /// <returns> Artist object </returns>
        public async Task<Artist> GetArtist(string id)
        {
            return await _db.Artist
                .Where(x => x.Id == id)
                .Include(a => a.ArtistPortfolios)
                .ThenInclude(b => b.Portfolio)
                .Include(c => c.ArtistBooking)
                .ThenInclude(d => d.Booking)
                .Select(y => new Artist
                {
                    Id = y.Id,
                    Name = y.Name,
                    Email = y.Email,
                    Intro = y.Intro,
                    Instagram = y.Instagram,
                    ProfileImageUri = y.ProfileImageUri,
                    ProfileImageFileName = y.ProfileImageFileName,
                    Order = y.Order,
                    Display = y.Display,
                    ArtistPortfolios = y.ArtistPortfolios,
                    ArtistBooking = y.ArtistBooking
                }).FirstOrDefaultAsync();
        }

        /// <summary>
        /// Helper method to reorder images when one is deleted.
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        private async Task ReOrderPortfolioImages(int portfolioId, int n)
        {
            IEnumerable<PortfolioImage> images = await GetPortfolioImages(portfolioId);
            foreach (PortfolioImage image in images)
            {
                if (image.Order > n)
                {
                    image.Order--;
                    _db.Entry(image).State = EntityState.Modified;
                    await _db.SaveChangesAsync();
                }
            }
        }

        /// <summary>
        /// Re-order portfolios when one is deleted.
        /// </summary>
        /// <param name="n"> portfolio removed </param>
        private async Task ReOrderPortfolios(string artistId, int n)
        {
            var portfolios = await GetArtistPortfolios(artistId);
            foreach (var portfolio in portfolios)
            {
                if (portfolio.Order > n)
                {
                    portfolio.Order--;
                    _db.Entry(portfolio).State = EntityState.Modified;
                    await _db.SaveChangesAsync();
                }
            }
        }
    }
}