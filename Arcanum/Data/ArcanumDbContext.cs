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
        public DbSet<PageImage> PageImage { get; set; }
        public DbSet<Artist> Artist { get; set; }
        public DbSet<RecentImage> RecentImage { get; set; }
        public DbSet<ArtistBooking> ArtistBooking { get; set; }
        public DbSet<ArtistPortfolio> ArtistPortfolio { get; set; }
        public DbSet<Image> Image { get; set; }
        public DbSet<StudioInfo> StudioInfo { get; set; }
        public DbSet<Address> Address { get; set; }
        public DbSet<StudioImage> StudioImage { get; set; }
        public DbSet<Booking> Booking { get; set; }
        public DbSet<Portfolio> Portfolio { get; set; }
        public DbSet<PortfolioImage> PortfolioImage { get; set; }
        public DbSet<RegistrationAccessCode> RegistrationAccessCodes { get; set; }

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
            modelBuilder.Entity<PageImage>().HasKey(x => new { x.ArcanumMainId, x.ImageId });

            SeedRole(modelBuilder, "WizardLord", "read", "create", "update", "delete");
            SeedRole(modelBuilder, "ArtistAdmin", "read", "create", "update", "delete");
            SeedRole(modelBuilder, "Guest", "read");

            string adminName = _config["SuperAdmin:UserName"];
            string adminPass = _config["SuperAdmin:Password"];
            string id = Guid.NewGuid().ToString();

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
                });

            modelBuilder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string>
                { 
                    RoleId = "wizardlord",
                    UserId = id
                });

            modelBuilder.Entity<ArcanumMain>().HasData(
                new ArcanumMain
                {
                    Id = -1,
                    SiteTitle = "Arcanum",
                    MainPageMessage = "I'm a pop up",
                    ShowHomePageMessage = false,
                    IntroA = "hello world",
                    IntroB = "here is some more info"
                });

            modelBuilder.Entity<Image>().HasData(
                new Image
                {
                    Id = -1,
                    SourceUrl = "https://via.placeholder.com/260x80",
                    Display = false,
                    AltText = "site-image"
                },
                new Image
                {
                    Id = -2,
                    SourceUrl = "https://via.placeholder.com/260x100",
                    Display = false,
                    AltText = "site-image"
                },
                new Image
                {
                    Id = -3,
                    SourceUrl = "https://via.placeholder.com/300x300",
                    Display = false,
                    AltText = "site-image"
                });

            modelBuilder.Entity<PageImage>().HasData(
                new PageImage
                {
                    ArcanumMainId = -1,
                    ImageId = -1,
                    Order = 0
                },
                new PageImage
                {
                    ArcanumMainId = -1,
                    ImageId = -2,
                    Order = 1
                },
                new PageImage
                {
                    ArcanumMainId = -1,
                    ImageId = -3,
                    Order = 2
                });


            modelBuilder.Entity<StudioInfo>().HasData(
                new StudioInfo
                {
                    Id = -1,
                    Email = "arcanumseattle@gmail.com",
                    Instagram = "@studioarcanum",
                    Intro = "here are some words",
                    Policies = "be nice",
                    Aftercare = " be smart",
                    ImageCount = 0
                });

            modelBuilder.Entity<Address>().HasData(
                new Address
                {
                    AddressId = -1,
                    Street = "4333 Fremont Ave N #6",
                    City = "Seattle",
                    State = "Wa",
                    ZipCode = 98103
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
                });

            modelBuilder.Entity<Booking>().HasData(
                new Booking
                {
                    Id = -1,
                    BookingInfo = "booking info",
                    BookingEmail = "scottfalboart@gmail.com"
                });

            modelBuilder.Entity<ArtistBooking>().HasData(
                new ArtistBooking
                {
                    ArtistId = id,
                    BookingId = -1
                });
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
