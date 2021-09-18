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
        public ISite _siteAdmin { get; set; }
        private readonly UserManager<ApplicationUser> _userManager;

        public ArtistModel(ISite siteAdmin, UserManager<ApplicationUser> userManager)
        {
            _siteAdmin = siteAdmin;
            _userManager = userManager;
        }

        public Artist Artist { get; set; }
        public string UserId { get; set; }

        public async Task OnGet(string artistId)
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
