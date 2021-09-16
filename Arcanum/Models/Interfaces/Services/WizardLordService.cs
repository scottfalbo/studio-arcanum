using Arcanum.Auth.Models;
using Arcanum.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Arcanum.Models.Interfaces.Services
{
    public class WizardLordService : IWizardLord
    {
        private readonly ArcanumDbContext _db;
        private readonly RoleManager<IdentityRole> _roleManager;

        public WizardLordService(ArcanumDbContext db, RoleManager<IdentityRole> roleManager)
        {
            _db = db;
            _roleManager = roleManager;
        }

        /// <summary>
        /// Queries the database for a list of registered users using Identity.
        /// Gets each user and attaches their assigned roles.
        /// </summary>
        /// <returns> List<ApplicationUserDto> </returns>
        public async Task<List<ApplicationUserDto>> GetRegisteredUsers()
        {
            List<ApplicationUserDto> users = await _db.Users
                .Select(x => new ApplicationUserDto
                {
                    Id = x.Id,
                    UserName = x.UserName,
                    Email = x.Email,
                }).ToListAsync();

            foreach (var user in users)
                user.Roles = await GetUserRoles(user.Id);

            return users;
        }

        /// <summary>
        /// Queries the database for all userRole join tables that have the userId.
        /// Iterates over the list of join tables and queries each Role by id.
        /// </summary>
        /// <param name="id"> string UserId </param>
        /// <returns> List<string> userRoles </returns>
        private async Task<List<string>> GetUserRoles(string id)
        {
            var roles = await _db.UserRoles
                .Where(x => x.UserId == id)
                .Select(y => new
                {
                    y.UserId,
                    y.RoleId
                }).ToListAsync();
            List<string> userRoles = new List<string>();
            foreach (var role in roles)
            {
                var roleObject = await GetRole(role.RoleId);
                userRoles.Add(roleObject.Name);
            }
            return userRoles;
        }

        /// <summary>
        /// Queries the database for a user role by id.
        /// </summary>
        /// <param name="id"> string RoleId </param>
        /// <returns> IdentityRole object </returns>
        public async Task<IdentityRole> GetRole(string id)
        {
            return await _db.Roles
                .Where(x => x.Id == id)
                .Select(y => new IdentityRole
                {
                    Id = y.Id,
                    Name = y.Name,
                    NormalizedName = y.NormalizedName
                }).FirstOrDefaultAsync();
        }

        /// <summary>
        /// Queries the database for a list of Identity roles.
        /// </summary>
        /// <returns> IQuerable<IdentityRole> roles </returns>
        public IQueryable<IdentityRole> GetRoles() =>
            _roleManager.Roles;
    }
}
