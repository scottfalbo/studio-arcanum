using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Arcanum.Auth.Models;
using Arcanum.Models;
using Arcanum.Models.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Arcanum.Pages
{
    public class ArtistModel : PageModel
    {
        private readonly ISite _siteAdmin;
        private readonly UserManager<ApplicationUser> _userManager;

        public ArtistModel(ISite siteAdmin, UserManager<ApplicationUser> userManager)
        {
            _siteAdmin = siteAdmin;
            _userManager = userManager;
        }

        [BindProperty]
        public Artist Artist { get; set; }
        [BindProperty]
        public string UserId { get; set; }

        public async Task OnGet(string artistId)
        {
            await RefreshPage(artistId);
        }

        public async Task OnPostUpdate()
        {
            await _siteAdmin.UpdateArtist(Artist);

            await RefreshPage(Artist.Id);
            Redirect($"Artist?artistId={UserId}");
        }

        private async Task RefreshPage(string artistId)
        {
            if (artistId != null)
                Artist = await _siteAdmin.GetArtist(artistId);

            if (User.IsInRole("ArtistAdmin"))
            {
                var userName = User.Identity.Name;
                var user = await _userManager.FindByNameAsync(userName);
                UserId = user.Id;
            }
            else
                UserId = "";
        }
    }
}
