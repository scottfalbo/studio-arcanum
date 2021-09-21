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
        private readonly IArtistAdmin _artistAdmin;
        private readonly UserManager<ApplicationUser> _userManager;

        public ArtistModel(ISite siteAdmin, IArtistAdmin artistAdmin, UserManager<ApplicationUser> userManager)
        {
            _siteAdmin = siteAdmin;
            _artistAdmin = artistAdmin;
            _userManager = userManager;
        }

        [BindProperty]
        public Artist Artist { get; set; }
        [BindProperty]
        public string UserId { get; set; }
        [BindProperty]
        public Portfolio Portfolio { get; set; }

        public async Task OnGet(string artistId)
        {
            await RefreshPage(artistId);
        }

        /// <summary>
        /// Method to update the general page data
        /// </summary>
        public async Task OnPostUpdate()
        {
            await _siteAdmin.UpdateArtist(Artist);

            await RefreshPage(Artist.Id);
            Redirect($"Artist?artistId={UserId}");
        }

        public async Task OnPostUpdatePortfolio(int index)
        {
            Portfolio.Intro = Artist.ArtistPortfolios[index].Portfolio.Intro;
            await _artistAdmin.UpdatePortfolio(Portfolio);

            await RefreshPage(Artist.Id);
            Redirect($"Artist?artistId={UserId}");
        }

        /// <summary>
        /// Helper method to refresh the page model properties
        /// </summary>
        /// <param name="artistId"> string artistId </param>
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
            Portfolio = new Portfolio();
        }
    }
}
