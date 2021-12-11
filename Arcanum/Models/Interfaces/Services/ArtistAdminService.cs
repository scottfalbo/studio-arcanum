using Arcanum.Data;
using Arcanum.ImageBlob.Interfaces;
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

        /// <summary>
        /// Delete the portfolio record.
        /// Remove the ArtistPortfolio and associated PortfolioImage join tables.
        /// </summary>
        /// <param name="portfolioId"> int portfolioId </param>
        /// <param name="artistId"> string artistId </param>
        public async Task DeletePortfolio(int portfolioId, string artistId)
        {
            List<PortfolioImage> images = await GetPortfolioImages(portfolioId);
            foreach(PortfolioImage image in images)
            {
                await RemoveImageFromPortfolio(portfolioId, image.ImageId);
                await DeleteImage(image.ImageId, portfolioId);
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
            ArtistPortfolio artistPortfolio = new ArtistPortfolio()
            {
                ArtistId = artistId,
                PortfolioId = portfolioId
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
                    ArtistId = artistId,
                    PortfolioId = portfolioId
                }).FirstOrDefaultAsync();
            _db.Entry(artistPortfolio).State = EntityState.Deleted;
            await _db.SaveChangesAsync();
        }

        /// <summary>
        /// Helper method to retrieve a portfolio record and return a list of it's relational images.
        /// </summary>
        /// <param name="id"> portfolio id </param>
        /// <returns> List<Image> </returns>
        public async Task<List<PortfolioImage>> GetPortfolioImages(int id)
        {
            Portfolio portfolio = await  _db.Portfolio
                .Where(a => a.Id == id)
                .Include(b => b.PortfolioImage)
                .ThenInclude(c => c.Image)
                .Select(x => new Portfolio
                {
                    PortfolioImage = x.PortfolioImage
                }).FirstOrDefaultAsync();
            return portfolio.PortfolioImage;
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
        /// Create a new image object and record in the database.
        /// </summary>
        /// <param name="image"> Image image </param>
        /// <returns> new Image object </returns>
        public async Task<Image> CreateImage(IFormFile file, string artistId, string title)
        {
            Artist artist = await GetArtist(artistId);
            Image image = await _upload.AddImage(file);
            Image newImage = new Image()
            {
                Title = title,
                Artist = artist.Name,
                SourceUrl = image.SourceUrl,
                ThumbnailUrl = image.ThumbnailUrl,
                FileName = image.FileName,
                ThumbFileName = image.ThumbFileName,
                Order = 0
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
                    ImageId = y.ImageId
                }).FirstOrDefaultAsync();
            _db.Entry(portfolioImage).State = EntityState.Deleted;
            await _db.SaveChangesAsync();
        }

        /// <summary>
        /// Removes image record, portfolio join table and files in storage blob.
        /// </summary>
        /// <param name="imageId"> int image id </param>
        /// <param name="portfolioId"> int portfolio id </param>
        /// <returns></returns>
        public async Task DeleteImage(int imageId, int portfolioId)
        {
            await RemoveImageFromPortfolio(portfolioId, imageId);
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
    }
}