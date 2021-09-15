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
                    Order = g.Order,
                    Email = g.Email,
                    Display = g.Display,
                    ArtistPortfolios = g.ArtistPortfolios,
                    ArtistBooking = g.ArtistBooking
                }).ToListAsync();
        }
    }
}
