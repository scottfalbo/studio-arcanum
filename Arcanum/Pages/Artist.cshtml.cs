using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Arcanum.Auth.Models;
using Arcanum.Auth.Models.Interfaces;
using Arcanum.Models;
using Arcanum.Models.Interfaces;
using Arcanum.Spells;
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
        [BindProperty]
        public UserNameUpdateState UserNameUpdateState { get; set; }
        public bool ActiveAdmin { get; set; }

        public async Task OnGet(string artistId, bool isActive = false, 
            PasswordUpdateState updatePasswordState = PasswordUpdateState.Inital,
            UserNameUpdateState updateUserNameState = UserNameUpdateState.Inital)
        {
            if (artistId != null)
                Artist = await _siteAdmin.GetArtist(artistId);

            if (User.IsInRole("ArtistAdmin") || User.IsInRole("WizardLord"))
            {
                //if (artistId == string.Empty)
                //{
                    var userName = User.Identity.Name;
                    var user = await _userManager.FindByNameAsync(userName);
                    UserId = user.Id;
                //}
                //else
                //{
                //    // bad logic
                //    UserId = artistId;
                //}

            }
            else
            {
                UserId = "";
            }

            Artist.SortPortfolioImages();
            Artist.SortArtistPortfolios();
            Portfolio = new Portfolio();
            PasswordUpdateState = updatePasswordState;
            UserNameUpdateState = updateUserNameState;
            ActiveAdmin = isActive;
        }

        /// <summary>
        /// Method to update the general page data
        /// </summary>
        public async Task<IActionResult> OnPostUpdate(string email)
        {
            await _siteAdmin.UpdateArtist(Artist);

            return Redirect($"Artist?artistId={Artist.Id}&isActive=true");
        }

        public async Task<IActionResult> OnPostUpdateProfilePhoto(IFormFile file)
        {
            await _artistAdmin.UpdateProfileImage(file, Artist.Id);

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
        public async Task<IActionResult> OnPostAddImages(IFormFile[] files)
        {
            foreach(IFormFile file in files)
            {
                Image image = await _artistAdmin.CreateImage(file, Artist.Id);
                await _artistAdmin.AddImageToPortfolio(Portfolio.Id, image.Id);
                await _artistAdmin.AddImageToRecent(-1, image.Id);
            }

            return Redirect($"Artist?artistId={Artist.Id}&isActive=true");
        }

        public async Task<IActionResult> OnPostUpdateImageInfo(string title, string altText, int imageId)
        {
            Image image = await _artistAdmin.GetImage(imageId);
            if (title != "")
                image.Title = title;
            if (altText != "")
                image.AltText = altText;
            await _artistAdmin.UpdateImage(image);

            return Redirect($"Artist?artistId={Artist.Id}&isActive=true");
        }

        public async Task<IActionResult> OnPostDeleteImage(int imageId, int portfolioId)
        {
            await _artistAdmin.RemoveImageFromPortfolio(portfolioId, imageId);
            await _artistAdmin.RemoveImageFromRecent(-1, imageId);
            await _artistAdmin.DeleteImage(imageId);

            return Redirect($"Artist?artistId={Artist.Id}&isActive=true");
        }

        public async Task<IActionResult> OnPostUpdateArtistPassword(string userId, string currentPassword, string newPassword)
        {
            var response = await _userService.UpdatePassword(userId, currentPassword, newPassword);
            PasswordUpdateState updatePasswordState = response.Succeeded ? PasswordUpdateState.Success : PasswordUpdateState.Failed;

            return Redirect($"Artist?artistId={userId}&isActive=true&updatePasswordState={updatePasswordState}");
        }

        public async Task<IActionResult> OnPostUpdateUserName(string userId, string newUserName)
        {
            var response = await _userService.UpdateUserName(userId, newUserName);
            var updateUserNameState = response.Succeeded ? UserNameUpdateState.Success : UserNameUpdateState.Failed;

            return Redirect($"Artist?artistId={userId}&isActive=true&updateUserNameState={updateUserNameState}");
        }

        public async Task<IActionResult> OnPostUpdatePortfolioImageOrder([FromBody] List<OrderSorter> imageOrder)
        {
            foreach(var image in imageOrder)
            {
                await _artistAdmin.UpdateImageOrder(image.Id, image.Order);
            }
            return new JsonResult(imageOrder);
        }

        public async Task<IActionResult> OnPostUpdatePortfolioOrder([FromBody] List<OrderSorter> portfolioOrder)
        {
            foreach (var portfolio in portfolioOrder)
            {
                await _artistAdmin.UpdatePortfolioOrder(portfolio.Id, portfolio.Order);
            }
            return new JsonResult(portfolioOrder);
        }
    }

    public enum PasswordUpdateState
    {
        Inital,
        Success,
        Failed
    }

    public enum UserNameUpdateState
    {
        Inital,
        Success,
        Failed
    }
    public enum UserEmailUpdateState
    {
        Inital,
        Success,
        Failed
    }
}
