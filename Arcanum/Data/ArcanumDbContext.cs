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
        public DbSet<Artist> Artist { get; set; }
        public DbSet<RecentImage> RecentImage { get; set; }
        public DbSet<ArtistBooking> ArtistBooking { get; set; }
        public DbSet<ArtistPortfolio> ArtistPortfolio { get; set; }
        public DbSet<Image> Image { get; set; }
        public DbSet<StudioInfo> StudioInfo { get; set; }
        public DbSet<Booking> Booking { get; set; }
        public DbSet<Portfolio> Portfolio { get; set; }
        public DbSet<PortfolioImage> PortfolioImage { get; set; }

        public IConfiguration Config { get; }

        public ArcanumDbContext(DbContextOptions options) : base(options) { }

        public ArcanumDbContext(DbContextOptions options, IConfiguration config) : base(options)
        {
            Config = config;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<RecentImage>().HasKey(x => new { x.ArcanumId, x.ImageId });
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

            modelBuilder.Entity<Artist>().HasData(
                new Artist 
                {
                    Id = "artist1",
                    Name = "tatter wizard",
                    Email = "wizard@wizarding.net",
                    Order = 1,
                    Display = true
                },
                new Artist 
                {
                    Id = "artist2",
                    Name = "tatter wizard 2",
                    Email = "wizard@wizarding.net",
                    Order = 2,
                    Display = true
                });

            modelBuilder.Entity<Booking>().HasData(
                new Models.Booking
                {
                    Id = -1,
                    BookingInfo = "booking info",
                    BookingEmail = "booking@booking.net"
                },
                new Models.Booking
                {
                    Id = -2,
                    BookingInfo = "booking info",
                    BookingEmail = "booking@booking.net"
                }
                );

            modelBuilder.Entity<ArtistBooking>().HasData(
                new ArtistBooking
                {
                    ArtistId = "artist1",
                    BookingId = -1
                },
                new ArtistBooking
                {
                    ArtistId = "artist2",
                    BookingId = -2
                }
                );

            modelBuilder.Entity<Portfolio>().HasData(
                new Portfolio {
                    Id = -1,
                    Title = "artist 1 portoflio",
                    Intro = "hi, I make tattoos",
                    Instagram = "@whatever"
                },
                new Portfolio
                {
                    Id = -2,
                    Title = "artist 2 portoflio",
                    Intro = "hi, I make tattoos",
                    Instagram = "@whatever2"
                }
                );

            modelBuilder.Entity<ArtistPortfolio>().HasData(
                new ArtistPortfolio
                {
                    ArtistId = "artist1",
                    PortfolioId = -1
                },
                new ArtistPortfolio
                {
                    ArtistId = "artist2",
                    PortfolioId = -2
                }
                );


            for (int i = -1; i > -21; i--)
            {
                modelBuilder.Entity<Image>().HasData(
                    new Image
                    {
                        Id = i,
                        Title = $"untitled-{i}",
                        Artist = "some one",
                        SourceUrl = "https://via.placeholder.com/60",
                        ThumbnailUrl = "https://via.placeholder.com/60",
                        FileName = "placeholder.png",
                        Order = Math.Abs(i)
                    });
            }


            for (int i = -1; i > -11; i--)
            {
                modelBuilder.Entity<PortfolioImage>().HasData(
                    new PortfolioImage
                    {
                        ImageId = i,
                        PortfolioId = -1
                    },
                    new PortfolioImage
                    {
                        ImageId = i - 10,
                        PortfolioId = -2
                    }
                    );
            }
        }
    }
}
