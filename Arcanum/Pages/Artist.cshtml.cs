using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Arcanum.Auth.Models;
using Arcanum.Auth.Models.Interfaces;
using Arcanum.Models;
using Arcanum.Models.Interfaces;
using Microsoft.AspNetCore.Http;
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
        private readonly IUserService _userService;

        public ArtistModel(ISite siteAdmin, IArtistAdmin artistAdmin, UserManager<ApplicationUser> userManager, IUserService userService)
        {
            _siteAdmin = siteAdmin;
            _artistAdmin = artistAdmin;
            _userManager = userManager;
            _userService = userService;
        }

        [BindProperty]
        public Artist Artist { get; set; }
        [BindProperty]
        public string UserId { get; set; }
        [BindProperty]
        public Portfolio Portfolio { get; set; }
        [BindProperty]
        public PasswordUpdateState PasswordUpdateState { get; set; }
        public bool ActiveAdmin { get; set; }

        public async Task OnGet(string artistId, bool isActive = false, PasswordUpdateState updateState = PasswordUpdateState.Inital)
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
            PasswordUpdateState = updateState;
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

        /// <summary>
        /// Add a portfolio record and join table.
        /// </summary>
        /// <param name="title"> string portfolio title </param>
        public async Task<IActionResult> OnPostAddPortfolio(string title)
        {
            Portfolio newPortfolio = await _artistAdmin.CreatePortfolio(title);
            await _artistAdmin.AddPortfolioToArtist(Artist.Id, newPortfolio.Id);
            return Redirect($"Artist?artistId={Artist.Id}&isActive=true");
        }

        /// <summary>
        /// Removes a portolio record and associated join tables.
        /// </summary>
        public async Task<IActionResult> OnPostDeletePortfolio()
        {
            await _artistAdmin.DeletePortfolio(Portfolio.Id, Artist.Id);
            return Redirect($"Artist?artistId={Artist.Id}&isActive=true");
        }

        /// <summary>
        /// Uploads images to blob storage, creates an image record and portfolio join table.
        /// </summary>
        /// <param name="files"> IFormFile[] input images </param>
        /// <param name="title"> string optional title </param>
        /// <returns></returns>
        public async Task<IActionResult> OnPostAddImages(IFormFile[] files, string title ="untitled")
        {
            foreach(IFormFile file in files)
            {
                Image image = await _artistAdmin.CreateImage(file, Artist.Id, title);
                await _artistAdmin.AddImageToPortfolio(Portfolio.Id, image.Id);
            }
            return Redirect($"Artist?artistId={Artist.Id}&isActive=true");
        }

        public async Task<IActionResult> OnPostDeleteImage(int imageId, int portfolioId)
        {
            await _artistAdmin.DeleteImage(imageId, portfolioId);
            return Redirect($"Artist?artistId={Artist.Id}&isActive=true");
        }

        public async Task<IActionResult> OnPostUpdateArtistPassword(string userId, string currentPassword, string newPassword)
        {
            var response = await _userService.UpdatePassword(userId, currentPassword, newPassword);
            PasswordUpdateState updateState = response.Succeeded ? PasswordUpdateState.Success : PasswordUpdateState.Failed;
            return Redirect($"Artist?artistId={userId}&isActive=true&updateState={updateState}");
        }
    }
    public enum PasswordUpdateState
    {
        Inital,
        Success,
        Failed
    }
}
