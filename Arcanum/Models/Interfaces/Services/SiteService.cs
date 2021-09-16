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

        public SiteService(ArcanumDbContext context)
        {
            _db = context;
        }

        /// <summary>
        /// Create a new artist record in the database with the user registration data.
        /// </summary>
        /// <param name="artist"> Artist object </param>
        public async Task CreateArtist(Artist artist)
        {
            Artist newArtist = new Artist()
            {
                Id = artist.Id,
                Name = artist.Name,
                Email = artist.Email,
                ProfileImageUri = "https://via.placeholder.com/200x300",
                Display = false,
                Order = 0
            };
            _db.Entry(newArtist).State = EntityState.Added;
            await _db.SaveChangesAsync();
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
            Artist artist = await _db.Artist.FindAsync(id);
            _db.Entry(artist).State = EntityState.Deleted;
            await _db.SaveChangesAsync();

            //TODO: Need to add logic to delete portfoios, images and booking page once they are built.
        }

    }
}
