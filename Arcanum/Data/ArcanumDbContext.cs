using Arcanum.Auth.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Arcanum.Models;

namespace Arcanum.Data
{
    public class ArcanumDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<ArcanumMain> Arcanum { get; set; }
        public DbSet<ArcanumArtist> ArcanumArtist { get; set; }
        public DbSet<Artist> Artist { get; set; }
        public DbSet<RecentImage> RecentImage { get; set; }
        public DbSet<ArcanumStudioInfo> ArcanumStudioInfo { get; set; }
        public DbSet<ArtistBooking> ArtistBooking { get; set; }
        public DbSet<ArtistPortfolio> ArtistPortfolio { get; set; }
        public DbSet<Image> Image { get; set; }
        public DbSet<StudioInfo> StudioInfo { get; set; }
        public DbSet<Booking> Booking { get; set; }
        public DbSet<Portfolio> Portfolio { get; set; }
        public DbSet<ArcanumMain> PortfolioImage { get; set; }

        public IConfiguration Config { get; }

        public ArcanumDbContext(DbContextOptions options) : base(options) { }

        public ArcanumDbContext(DbContextOptions options, IConfiguration config) : base(options)
        {
            Config = config;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
