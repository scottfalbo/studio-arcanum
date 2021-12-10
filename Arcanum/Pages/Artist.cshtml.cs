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
        public bool ActiveAdmin { get; set; }

        public async Task OnGet(string artistId, bool isActive = false)
        {
            if (artistId != null)
                Artist = await _siteAdmin.GetArtist(artistId);

            if (User.IsInRole("ArtistAdmin") || User.IsInRole("WizardLord"))
            {
                var userName = User.Identity.Name;
                var user = await _userManager.FindByNameAsync(userName);
                UserId = user.Id;
            }
            else
                UserId = "";
            Portfolio = new Portfolio();
            ActiveAdmin = isActive;
        }

        /// <summary>
        /// Method to update the general page data
        /// </summary>
        public async Task<IActionResult> OnPostUpdate()
        {
            await _siteAdmin.UpdateArtist(Artist);
            return Redirect($"Artist?artistId={Artist.Id}&isActive=true");
        }

        /// <summary>
        /// Update the general portfolio fields.
        /// </summary>
        /// <param name="index"> index for ArtistPortfolio List </param>
        public async Task<IActionResult> OnPostUpdatePortfolio(string intro)
        {
            Portfolio.Intro = intro;
            await _artistAdmin.UpdatePortfolio(Portfolio);
            return Redirect($"Artist?artistId={Artist.Id}&isActive=true");
        }

        public async Task<IActionResult> OnPostAddPortfolio(string title)
        {
            Portfolio newPortfolio = await _artistAdmin.CreatePortfolio(title);
            await _artistAdmin.AddPortfolioToArtist(Artist.Id, newPortfolio.Id);
            return Redirect($"Artist?artistId={Artist.Id}&isActive=true");
        }

        public async Task<IActionResult> OnPostDeletePortfolio()
        {
            await _artistAdmin.DeletePortfolio(Portfolio.Id, Artist.Id);
            return Redirect($"Artist?artistId={Artist.Id}&isActive=true");
        }
    }
}
