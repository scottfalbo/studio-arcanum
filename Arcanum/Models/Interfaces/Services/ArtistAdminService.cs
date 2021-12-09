using Arcanum.Data;
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

        public ArtistAdminService(ArcanumDbContext db)
        {
            _db = db;
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
                Order = 0,
                Display = false
            };
            newPortfolio = AssignAccordianIds(newPortfolio);
            _db.Entry(newPortfolio).State = EntityState.Added;
            await _db.SaveChangesAsync();
            return newPortfolio;
        }

        /// <summary>
        /// Update the portfolio record in the database.
        /// Reassigns accordion class names incase the portfolio title changed.
        /// </summary>
        /// <param name="portfolio"> Portfolio object </param>
        public async Task UpdatePortfolio(Portfolio portfolio)
        {
            portfolio = AssignAccordianIds(portfolio);
            _db.Entry(portfolio).State = EntityState.Modified;
            await _db.SaveChangesAsync();
        }

        public async Task DeletePortfolio(int portfolioId, string artistId)
        {
            
        }

        /// <summary>
        /// Helper method fo retrieve a portfolio record and return a list of it's relational images.
        /// </summary>
        /// <param name="id"> portfolio id </param>
        /// <returns> List<Image> </returns>
        private async Task<List<Image>> GetPortfolioImages(int id)
        {
            Portfolio portfolio = await  _db.Portfolio
                .Where(a => a.Id == id)
                .Include(b => b.PortfolioImage)
                .ThenInclude(c => c.Image)
                .Select(x => new Portfolio
                {
                    PortfolioImage = x.PortfolioImage
                }).FirstOrDefaultAsync();
            List<Image> images = new List<Image>();
            foreach(PortfolioImage image in portfolio.PortfolioImage)
            {
                images.Add(image.Image);
            }
            return images;
        }

        /// <summary>
        /// Helper method to assign some unique names for use with Bootstrap accordion menues.
        /// </summary>
        /// <param name="portfolio"> Portfolio object </param>
        /// <returns> Portfolio object w/accordion ids </returns>
        private Portfolio AssignAccordianIds(Portfolio portfolio)
        {
            string str = Regex.Replace(portfolio.Title, @"\s+", String.Empty).ToLower();

            portfolio.AccordionId = str;
            portfolio.CollapseId = $"{str}{portfolio.Id}";
            portfolio.AdminAccordionId = $"{str}admin";
            portfolio.AdminCollapseId = $"{str}{portfolio.Id}admin";

            return portfolio;
        }

        /// <summary>
        /// Add a portfolio object to an artist with a PortfolioArtist join table.
        /// </summary>
        /// <param name="artistId"> string artistId </param>
        /// <param name="portfolioId"> int portfolioId </param>
        public async Task AddPortfolioToArtist(string artistId, int portfolioId)
        {
            ArtistPortfolio artistPortfolio = new ArtistPortfolio()
            {
                ArtistId = artistId,
                PortfolioId = portfolioId
            };
            _db.Entry(artistPortfolio).State = EntityState.Added;
            await _db.SaveChangesAsync();
        }

        /// <summary>
        /// Create a new image object and record in the database.
        /// </summary>
        /// <param name="image"> Image image </param>
        /// <returns> new Image object </returns>
        public async Task<Image> CreateImage(Image image)
        {
            Image newImage = new Image()
            {
                Title = image.Title,
                Artist = image.Artist,
                SourceUrl = image.SourceUrl,
                ThumbnailUrl = image.ThumbnailUrl,
                FileName = image.FileName,
                ThumbFileName = image.ThumbFileName,
                Order = image.Order
            };
            _db.Entry(newImage).State = EntityState.Added;
            await _db.SaveChangesAsync();
            return newImage;
        }

        /// <summary>
        /// Add an image to a portfolio with a PortfolioImage join table record.
        /// </summary>
        /// <param name="portfolioId"> int portfolio id </param>
        /// <param name="imageId"> int image id </param>
        /// <returns></returns>
        public async Task AddImageToPortfolio(int portfolioId, int imageId)
        {
            PortfolioImage portfolioImage = new PortfolioImage()
            {
                PortfolioId = portfolioId,
                ImageId = imageId
            };
            _db.Entry(portfolioImage).State = EntityState.Added;
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
        /// Update the booking record in the database.
        /// </summary>
        /// <param name="booking"> Booking object </param>
        public async Task UpdateArtistBooking(Booking booking)
        {
            _db.Entry(booking).State = EntityState.Modified;
            await _db.SaveChangesAsync();
        }
    }
}
