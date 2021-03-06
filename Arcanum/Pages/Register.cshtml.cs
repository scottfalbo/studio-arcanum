using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Arcanum.Auth.Models;
using Arcanum.Auth.Models.Interfaces;
using Arcanum.Models;
using Arcanum.Models.Interfaces;
using Arcanum.Spells;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Arcanum.Pages
{
    public class RegisterModel : PageModel
    {
        public UserManager<ApplicationUser> _userManager;
        public IUserService _userService;
        public ISite _siteAdmin;
        public IArtistAdmin _artistAdmin;
        public IWizardLord _wizard;

        public RegisterModel(UserManager<ApplicationUser> userManager, IUserService userService, ISite siteAdmin, IArtistAdmin artistAdmin, IWizardLord wizard)
        {
            _userManager = userManager;
            _userService = userService;
            _siteAdmin = siteAdmin;
            _artistAdmin = artistAdmin;
            _wizard = wizard;
        }

        public List<RegistrationAccessCode> AccessCodes { get; set; }
        public PageState PageState { get; set; }

        [BindProperty]
        public string ValidCode { get; set; }
        public List<string> Users { get; set; }
        [BindProperty]
        public bool IsRegistrationSuccess { get; set; }
        public string[] RegistrationErrorMessage { get; set; }

        public async Task OnGet(bool isRegistrationSuccess = true, PageState pageState = PageState.Inital, string errors = "", string validationCode = "")
        {
            IsRegistrationSuccess = isRegistrationSuccess;
            RegistrationErrorMessage = errors.Split(", ");
            Users = MinorSpells.ConvertUsers(_userManager.Users);
            AccessCodes = await _wizard.GetRegistrationAccessCodes();
            PageState = pageState;
            ValidCode = validationCode;
        }

        public async Task<IActionResult> OnPostRegister(string username, string password, string email)
        {
            username = Regex.Replace(username, " ", "");
            RegisterUser newUser = new RegisterUser()
            {
                UserName = username,
                Password = password,
                Email = email
            };

            ApplicationUserDto user = await _userService.Register(newUser, this.ModelState);

            if (user.IsSuccessfullyRegistered)
            {
                Artist artist = await _siteAdmin.CreateArtist(NewArtist(user));
                if (artist != null)
                {
                    await _wizard.DeleteRegistrationAccessCode(ValidCode);
                }
                return Redirect("/Login");
            }
            else
            {
                return Redirect($"/Register?isRegistrationSuccess={false}&pageState={PageState.Confirmed}&errors={user.RegistrationErrors}&validationCode={ValidCode}");
            }

        }

        public async Task OnPostValidateAccessCode(string code)
        {
            RegistrationAccessCode accessCode = await _wizard.GetRegistrationAccessCode(code);
            if (accessCode == null)
            {
                PageState = PageState.NotValid;
            }
            else
            {
                PageState = PageState.Confirmed;
                ValidCode = code;
                IsRegistrationSuccess = true;
            }
            Redirect("/Registration");
        }

        /// <summary>
        /// Helper method to instantiate a new Arist object.
        /// </summary>
        /// <param name="user"> ApplicationUserDto newUser </param>
        /// <returns> new Artist object </returns>
        private Artist NewArtist(ApplicationUserDto user)
        {
            return new Artist()
            {
                Id = user.Id,
                Name = user.UserName,
                Email = user.Email
            };
        }
    }

    public enum PageState
    {
        Inital,
        NotValid,
        Confirmed
    }
}
