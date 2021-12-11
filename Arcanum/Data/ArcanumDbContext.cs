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
