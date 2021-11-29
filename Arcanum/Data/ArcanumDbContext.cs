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
        public DbSet<ArcanumMain> ArcanumMain { get; set; }
        public DbSet<Artist> Artist { get; set; }
        public DbSet<RecentImage> RecentImage { get; set; }
        public DbSet<ArtistBooking> ArtistBooking { get; set; }
        public DbSet<ArtistPortfolio> ArtistPortfolio { get; set; }
        public DbSet<Image> Image { get; set; }
        public DbSet<StudioInfo> StudioInfo { get; set; }
        public DbSet<Booking> Booking { get; set; }
        public DbSet<Portfolio> Portfolio { get; set; }
        public DbSet<PortfolioImage> PortfolioImage { get; set; }

        public IConfiguration _config { get; }

        public ArcanumDbContext(DbContextOptions options) : base(options) { }

        public ArcanumDbContext(DbContextOptions options, IConfiguration config) : base(options)
        {
            _config = config;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<RecentImage>().HasKey(x => new { x.ArcanumMainId, x.ImageId });
            modelBuilder.Entity<ArtistBooking>().HasKey(x => new { x.ArtistId, x.BookingId });
            modelBuilder.Entity<ArtistPortfolio>().HasKey(x => new { x.ArtistId, x.PortfolioId });
            modelBuilder.Entity<PortfolioImage>().HasKey(x => new { x.PortfolioId, x.ImageId });
            modelBuilder.Entity<StudioImage>().HasKey(x => new { x.StudioInfoId, x.ImageId });

            SeedRole(modelBuilder, "WizardLord", "read", "create", "update", "delete");
            SeedRole(modelBuilder, "ArtistAdmin", "read", "create", "update", "delete");
            SeedRole(modelBuilder, "Guest", "read");

            string id = _config["SuperAdmin:UserId"];
            string adminName = _config["SuperAdmin:UserName"];
            string adminPass = _config["SuperAdmin:Password"];
            PasswordHasher<ApplicationUser> hasher = new PasswordHasher<ApplicationUser>();

            modelBuilder.Entity<ApplicationUser>().HasData(
                new ApplicationUser
                {
                    Id = id,
                    UserName = adminName,
                    NormalizedUserName = adminName.ToUpper(),
                    Email = "scottfalboart@gmail.com",
                    NormalizedEmail = "scottfalboart@gmail.com",
                    EmailConfirmed = true,
                    PasswordHash = hasher.HashPassword(null, adminPass),
                    SecurityStamp =  string.Empty
                },
                new ApplicationUser
                {
                    Id = "artist1",
                    UserName = "luci",
                    NormalizedUserName = "LUCI",
                    Email = "whatever@whatever.com",
                    NormalizedEmail = "whatever@whatever.com",
                    EmailConfirmed = false,
                    PasswordHash = hasher.HashPassword(null, "Pass!23"),
                    SecurityStamp = string.Empty
                },
                new ApplicationUser
                {
                    Id = "artist2",
                    UserName = "harry",
                    NormalizedUserName = "HARRY",
                    Email = "whatever2@whatever.com",
                    NormalizedEmail = "whatever2@whatever.com",
                    EmailConfirmed = false,
                    PasswordHash = hasher.HashPassword(null, "Pass!23"),
                    SecurityStamp = string.Empty
                });

            modelBuilder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string>
                { 
                    RoleId = "wizardlord",
                    UserId = id
                },
                new IdentityUserRole<string>
                {
                    RoleId = "artistadmin",
                    UserId = "artist1"
                },
                new IdentityUserRole<string>
                {
                    RoleId = "artistadmin",
                    UserId = "artist2"
                });

            modelBuilder.Entity<ArcanumMain>().HasData(
                new ArcanumMain
                {
                    Id = -1,
                    SiteTitle = "Arcanum",
                    IntroA = "hello world",
                    IntroB = "here is some more info"
                });


            modelBuilder.Entity<StudioInfo>().HasData(
                new StudioInfo
                {
                    Id = -1,
                    Instagram = "@studioarcanum",
                    Intro = "here are some words",
                    Address = "some where",
                    Policies = "be nice",
                    Aftercare = " be smart"
                });

            modelBuilder.Entity<Artist>().HasData(
                new Artist
                {
                    Id = id,
                    Name = adminName,
                    Email = "scottfalboart@gmail.com",
                    Intro = "I do tattoos",
                    Instagram = "@scottfalboart",
                    ProfileImageUri = "https://via.placeholder.com/200x300",
                    Order = 1,
                    Display = true
                },
                new Artist 
                {
                    Id = "artist1",
                    Name = "luci",
                    Email = "wizard@wizarding.net",
                    Order = 2,
                    Display = true
                },
                new Artist 
                {
                    Id = "artist2",
                    Name = "harry",
                    Email = "wizard@wizarding.net",
                    Order = 3,
                    Display = true
                });

            modelBuilder.Entity<Booking>().HasData(
                new Booking
                {
                    Id = -1,
                    BookingInfo = "booking info",
                    BookingEmail = "scottfalboart@gmail.com"
                },
                new Booking
                {
                    Id = -2,
                    BookingInfo = "booking info",
                    BookingEmail = "booking@booking.net"
                },
                new Booking
                {
                    Id = -3,
                    BookingInfo = "booking info",
                    BookingEmail = "booking@booking.net"
                });

            modelBuilder.Entity<ArtistBooking>().HasData(
                new ArtistBooking
                {
                    ArtistId = id,
                    BookingId = -1
                },
                new ArtistBooking
                {
                    ArtistId = "artist1",
                    BookingId = -2
                },
                new ArtistBooking
                {
                    ArtistId = "artist2",
                    BookingId = -3
                });

            modelBuilder.Entity<Portfolio>().HasData(
                new Portfolio
                {
                    Id = -1,
                    Title = $"{adminName}'s portoflio",
                    Intro = "hi, I make tattoos",
                    Display = true
                },
                new Portfolio
                {
                    Id = -2,
                    Title = "artist 1 portoflio",
                    Intro = "hi, I make tattoos",
                    Display = true
                },
                new Portfolio
                {
                    Id = -3,
                    Title = "artist 2 portoflio",
                    Intro = "hi, I make tattoos",
                    Display = true
                });

            modelBuilder.Entity<ArtistPortfolio>().HasData(
                new ArtistPortfolio
                {
                    ArtistId = id,
                    PortfolioId = -1
                },
                new ArtistPortfolio
                {
                    ArtistId = "artist1",
                    PortfolioId = -2
                },
                new ArtistPortfolio
                {
                    ArtistId = "artist2",
                    PortfolioId = -3
                });

            for (int i = -1; i > -41; i--)
            {
                modelBuilder.Entity<Image>().HasData(
                    new Image
                    {
                        Id = i,
                        Title = $"untitled-{i}",
                        Artist = "some one",
                        SourceUrl = "https://via.placeholder.com/800x1200",
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
                    },
                    new PortfolioImage
                    {
                        ImageId = i - 20,
                        PortfolioId = -3
                    });

                modelBuilder.Entity<RecentImage>().HasData(
                    new RecentImage
                    {
                        ArcanumMainId = -1,
                        ImageId = i
                    });

                modelBuilder.Entity<StudioImage>().HasData(
                    new StudioImage
                    {
                        StudioInfoId = -1,
                        ImageId = i - 30
                    });
            }
        }

        private int id = 1;
        private void SeedRole(ModelBuilder modelBuilder, string roleName, params string[] permissions)
        {
            var role = new IdentityRole
            {
                Id = roleName.ToLower(),
                Name = roleName,
                NormalizedName = roleName.ToUpper(),
                ConcurrencyStamp = Guid.Empty.ToString()
            };
            modelBuilder.Entity<IdentityRole>().HasData(role);

            var roleClaims = permissions.Select(permission =>
               new IdentityRoleClaim<string>
               {
                   Id = id++,
                   RoleId = role.Id,
                   ClaimType = "permissions",
                   ClaimValue = permission
               });
            modelBuilder.Entity<IdentityRoleClaim<string>>().HasData(roleClaims);
        }
    }
}
