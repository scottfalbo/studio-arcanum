using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Arcanum.Auth.Models;
using Arcanum.Auth.Models.Interfaces;
using Arcanum.Models;
using Arcanum.Models.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Arcanum.Pages
{
    public class RegisterModel : PageModel
    {
        public IUserService _userService;
        public ISite _siteAdmin;

        public RegisterModel(IUserService userService, ISite siteAdmin)
        {
            _userService = userService;
            _siteAdmin = siteAdmin;
        }
        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync(string username, string password, string email)
        {
            RegisterUser newUser = new RegisterUser()
            {
                UserName = username,
                Password = password,
                Email = email
            };

            ApplicationUserDto user = await _userService.Register(newUser, this.ModelState);
            //await _siteAdmin.CreateArtist(NewArtist(user));

            return Redirect("/Login");
        }

        private Artist NewArtist(ApplicationUserDto user)
        {
            return new Artist()
            {
                Id = user.Id,
                Name = user.UserName,
                Email = user.Email,
                Display = true
            };
        }
    }
}
