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
            throw new Exception("womp womp");
        }

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
                await _userManager.AddToRolesAsync(user, new List<string>() { "Guest" });

                return new ApplicationUserDto
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                    Roles = new List<string>() { "Guest" },
                };
            }
            return null;
        }
    }
}

