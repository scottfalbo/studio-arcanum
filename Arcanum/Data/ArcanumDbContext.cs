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

            modelBuilder.Entity<ArcanumArtist>().HasKey(x => new { x.ArcanumId, x.ArtistId });
            modelBuilder.Entity<RecentImage>().HasKey(x => new { x.ArcanumId, x.ImageId });
            modelBuilder.Entity<ArcanumStudioInfo>().HasKey(x => new { x.ArcanumId, x.StudioInfoId });
            modelBuilder.Entity<ArtistBooking>().HasKey(x => new { x.ArtistId, x.BookingId });
            modelBuilder.Entity<ArtistPortfolio>().HasKey(x => new { x.ArtistId, x.PortfolioId });
            modelBuilder.Entity<PortfolioImage>().HasKey(x => new { x.PortfolioId, x.ImageId });

            modelBuilder.Entity<ArcanumMain>().HasData(
                new ArcanumMain
                {
                    Id = -1,
                    Intro = "hello world",
                });

            modelBuilder.Entity<StudioInfo>().HasData(
                new StudioInfo
                {
                    Id = -1,
                    Address = "some where",
                    Policies = "be nice",
                    Aftercare = " be smart"
                });

            modelBuilder.Entity<ArcanumStudioInfo>().HasData(
                new ArcanumStudioInfo
                {
                    ArcanumId = -1,
                    StudioInfoId = -1
                });

            modelBuilder.Entity<Artist>().HasData(
                new Artist 
                {
                    Id = "artist",
                    Name = "tatter wizard",
                    Email = "wizard@wizarding.net",
                    Order = 1,
                    Display = true
                });

            modelBuilder.Entity<ArcanumArtist>().HasData(
                new ArcanumArtist
                {
                    ArcanumId = -1,
                    ArtistId = "artist"
                });

            modelBuilder.Entity<Booking>().HasData(
                new Models.Booking
                {
                    Id = -1,
                    BookingInfo = "booking info",
                    BookingEmail = "booking@booking.net"
                });

            modelBuilder.Entity<ArtistBooking>().HasData(
                new ArtistBooking
                {
                    ArtistId = "artist",
                    BookingId = -1
                });

            modelBuilder.Entity<Portfolio>().HasData(
                new Portfolio {
                    Id = -1,
                    Title = "some one's portoflio",
                    Intro = "hi, I make tattoos",
                    Instagram = "@whatever"
                });

            modelBuilder.Entity<ArtistPortfolio>().HasData(
                new ArtistPortfolio
                {
                    ArtistId = "artist",
                    PortfolioId = -1
                });

            modelBuilder.Entity<Image>().HasData(
                new Image
                {
                    Id = -1,
                    Title = "untitled",
                    Artist = "some one",
                    SourceUrl = "https://via.placeholder.com/600",
                    ThumbnailUrl = "https://via.placeholder.com/60",
                    FileName = "palceholder.png",
                    Order = 1
                });

            modelBuilder.Entity<PortfolioImage>().HasData(
                new PortfolioImage
                { 
                    ImageId = -1,
                    PortfolioId = -1
                });

            modelBuilder.Entity<RecentImage>().HasData(
                new RecentImage
                {
                    ArcanumId = -1,
                    ImageId = -1
                });

        }
    }
}
