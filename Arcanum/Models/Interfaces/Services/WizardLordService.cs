using Arcanum.Auth.Models;
using Arcanum.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Arcanum.Spells;

namespace Arcanum.Models.Interfaces.Services
{
    public class WizardLordService : IWizardLord
    {
        private readonly ArcanumDbContext _db;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public WizardLordService(ArcanumDbContext db, RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _roleManager = roleManager;
            _userManager = userManager;
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

        /// <summary>
        /// Helper method to get just the role names from the IQuerable Roles object
        /// </summary>
        /// <param name="roles"> IQueryable<IdentityRole> roles </param>
        /// <returns> List<string> roleNames </returns>
        private List<string> IdentityRoleToString(IQueryable<IdentityRole> roles)
        {
            List<string> roleNames = new List<string>();
            foreach (IdentityRole role in roles)
                roleNames.Add(role.Name);
            return roleNames;
        }

        /// <summary>
        /// Add and remove roles from a user based on check box form input.
        /// </summary>
        /// <param name="userId"> string userId </param>
        /// <param name="roles"> string[] selected roles </param>
        public async Task UpdateUserRoles(string userId, string[] roles)
        {
            var user = await _userManager.FindByIdAsync(userId);
            IEnumerable<string> currentRoles = GetUserRoles(userId).Result;

            await _userManager.RemoveFromRolesAsync(user, currentRoles);
            await _userManager.AddToRolesAsync(user, roles);
        }

        /// <summary>
        /// Delete a registered user and remove the respective role join tables.
        /// </summary>
        /// <param name="userId"> string userId </param>
        public async Task DeleteUser(string userId)
        {
            var roles = GetRoles();
            var roleNames = IdentityRoleToString(roles);
            var user = await _userManager.FindByIdAsync(userId);
            await _userManager.RemoveFromRolesAsync(user, roleNames);
            await _userManager.DeleteAsync(user);
        }

        /// <summary>
        /// Instantiate a new RegistrationAccessCode object and add a record to the database.
        /// </summary>
        /// <param name="code"> string access code </param>
        /// <returns> RegistrationAccessCode object </returns>
        public async Task<RegistrationAccessCode> CreateRegistrationAccessCode(string name)
        {
            string code = NameHasher.HashNameToAccessCode(name);
            RegistrationAccessCode accessCode = new RegistrationAccessCode()
            {
                Code = code,
                Name = name
            };
            _db.Entry(accessCode).State = EntityState.Added;
            await _db.SaveChangesAsync();
            return accessCode;
        }

        /// <summary>
        /// Retrieve an access code from the data base by code.
        /// </summary>
        /// <param name="code"> string access code </param>
        /// <returns> RegistrationAccessCode object </returns>
        public async Task<RegistrationAccessCode> GetRegistrationAccessCode(string code)
        {
            return await _db.RegistrationAccessCodes
                .Where(x => x.Code == code)
                .Select(y => new RegistrationAccessCode
                {
                    Id = y.Id,
                    Code = y.Code,
                    Name = y.Name
                }).FirstOrDefaultAsync();
        }

        /// <summary>
        /// Retrieve a list of all RegistrationAccessCode records.
        /// </summary>
        /// <returns> List<RegistrationAccessCode> </returns>
        public async Task<List<RegistrationAccessCode>> GetRegistrationAccessCodes()
        {
            return await _db.RegistrationAccessCodes
                .Select(x => new RegistrationAccessCode
                {
                    Id = x.Id,
                    Code = x.Code,
                    Name = x.Name
                }).ToListAsync();
        }

        /// <summary>
        /// Remove an access code record from the data base.
        /// </summary>
        /// <param name="code"> string access code </param>
        public async Task DeleteRegistrationAccessCode(string code)
        {
            RegistrationAccessCode accessCode = await GetRegistrationAccessCode(code);
            _db.Entry(accessCode).State = EntityState.Deleted;
            await _db.SaveChangesAsync();
        }
    }
}
