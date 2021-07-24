using Arcanum.Auth.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Arcanum.Data
{
    public class ArcanumDbContext : IdentityDbContext<ApplicationUser>
    {
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
