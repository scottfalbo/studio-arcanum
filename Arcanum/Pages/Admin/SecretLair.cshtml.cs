using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Arcanum.Auth.Models;
using Arcanum.Data;
using Arcanum.Models;
using Arcanum.Models.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Arcanum.Pages.Admin
{
    public class SecretLairModel : PageModel
    {
        public ISite _siteAdmin;
        public IWizardLord _wizard;

        public SecretLairModel(ISite siteAdmin, IWizardLord magicPower)
        {
            _siteAdmin = siteAdmin;
            _wizard = magicPower;
        }

        public List<ApplicationUserDto> Users { get; set; }
        public IQueryable<IdentityRole> Roles { get; set; }
        public List<Artist> Artists { get; set; }
        public List<RegistrationAccessCode> AccessCodes { get; set; }

        public async Task OnGet()
        {
            Users = await _wizard.GetRegisteredUsers();
            Roles = _wizard.GetRoles();
            Artists = await _siteAdmin.GetArtists();
            AccessCodes = await _wizard.GetRegistrationAccessCodes();
        }

        public async Task<IActionResult> OnPostUpdateUserRoles(string userId, string[] isChecked)
        {
            await _wizard.UpdateUserRoles(userId, isChecked);
            return Redirect("/Admin/SecretLair");
        }

        public async Task<IActionResult> OnPostDeleteUser(string userId)
        {
            await _wizard.DeleteUser(userId);
            await _siteAdmin.DeleteArtist(userId);
            return Redirect("/Admin/SecretLair");
        }

        public async Task<IActionResult> OnPostToggleArtistDisplay(bool display, string artistId)
        {
            Artist artist = await _siteAdmin.GetArtist(artistId);
            artist.Display = display;
            await _siteAdmin.UpdateArtist(artist);
            return Redirect("/Admin/SecretLair");
        }

        public async Task<IActionResult> OnPostCreateRegistrationCode(string code)
        {
            await _wizard.CreateRegistrationAccessCode(code);
            return Redirect("/Admin/SecretLair");
        }
    }
}
