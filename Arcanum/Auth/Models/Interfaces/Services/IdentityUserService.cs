using Arcanum.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Arcanum.Auth.Models.Interfaces.Services
{
    public class IdentityUserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signin;
        private readonly ArcanumDbContext _db;

        public IdentityUserService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signin, ArcanumDbContext db)
        {
            _userManager = userManager;
            _signin = signin;
            _db = db;
        }

        /// <summary>
        /// Authenticates a user at login.
        /// </summary>
        /// <param name="userName"> string userName</param>
        /// <param name="password"></param>
        /// <returns> ApplicationUserDto of authenticated user </returns>
        public  async Task<ApplicationUserDto> Authenticate(string userName, string password)
        {
            var result = await _signin.PasswordSignInAsync(userName, password, true, false);

            if (result.Succeeded)
            {
                var user = await _userManager.FindByNameAsync(userName);


                return new ApplicationUserDto
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                    Roles = await _userManager.GetRolesAsync(user),
                };
            }
            return null;
        }

        /// <summary>
        /// Registers a new user identity.
        /// </summary>
        /// <param name="data"> RegisterUser object </param>
        /// <param name="modelState"></param>
        /// <returns> ApplicationUserCto of the new user </returns>
        public async Task<ApplicationUserDto> Register(RegisterUser data, ModelStateDictionary modelState)
        {
            var user = new ApplicationUser()
            {
                UserName = data.UserName,
                Email = data.Email
            };
            var result = await _userManager.CreateAsync(user, data.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRolesAsync(user, new List<string>() { "ArtistAdmin" });

                return new ApplicationUserDto
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                    Roles = new List<string>() { "ArtistAdmin" }
                };
            }
            return null;
        }

        /// <summary>
        /// Updates a users password.
        /// </summary>
        /// <param name="userId"> string userId </param>
        /// <param name="currentPassword"> string currentPassword </param>
        /// <param name="newPassword"> string newPassword </param>
        /// <returns></returns>
        public async Task<IdentityResult> UpdatePassword(string userId, string currentPassword, string newPassword)
        {
            var user = await _userManager.FindByIdAsync(userId);
            return await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);
        }

        public async Task<IdentityResult> UpdateUserName(string userId, string newName)
        {
            var user = await _userManager.FindByIdAsync(userId);
            user.UserName = newName;
            return await _userManager.UpdateAsync(user);
        }

        public async Task<IdentityResult> UpdateUserEmail(string userId, string newEmail)
        {
            var user = await _userManager.FindByIdAsync(userId);
            return await _userManager.SetEmailAsync(user, newEmail);
        }
    }
}

