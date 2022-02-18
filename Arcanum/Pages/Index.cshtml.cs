using Arcanum.Models;
using Arcanum.Models.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Arcanum.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly ISite _siteAdmin;
        private readonly IArtistAdmin _artistAdmin;

        public IndexModel(ILogger<IndexModel> logger, ISite siteAdmin, IArtistAdmin artistAdmin)
        {
            _logger = logger;
            _siteAdmin = siteAdmin;
            _artistAdmin = artistAdmin;
        }

        [BindProperty]
        public ArcanumMain MainPage { get; set; }
        public bool ActiveAdmin { get; set; }
        [BindProperty]
        public IEnumerable<Artist> Artists { get; set; }
        [BindProperty]
        public StudioInfo StudioInfo { get; set; }

        public async Task OnGet(bool isActive = false)
        {
            MainPage = await _siteAdmin.GetMainPage();
            IEnumerable<PageImage> pageImages = MainPage.PageImage;
            MainPage.PageImage = (pageImages.OrderBy(x => x.Order)).ToList();
            Artists = await _siteAdmin.GetArtists();
            StudioInfo = await _siteAdmin.GetStudio();
            ActiveAdmin = isActive;
        }

        /// <summary>
        /// Update the main page text area
        /// </summary>
        public async Task<IActionResult> OnPostUpdate()
        {
            await _siteAdmin.UpdateMainPage(MainPage);

            return Redirect("/Index?isActive=true");
        }

        /// <summary>
        /// Updates the images on the main page.
        /// </summary>
        public async Task<IActionResult> OnPostUpdatePageImage(IFormFile file, int index, int imageId)
        {
            Image image = await _siteAdmin.UpdateMainPageImage(file);
            await _siteAdmin.AddImageToMainPage(-1, image.Id, index);
            await _siteAdmin.RemoveImageFromMainPage(-1, imageId);
            await _artistAdmin.DeleteImage(imageId);

            return Redirect("/Index?isActive=true");
        }

        /// <summary>
        /// Update images diplay option.
        /// </summary>
        public async Task<IActionResult> OnPostDisplayImageToggle(bool isDisplay, int imageId)
        {
            Image image = await _artistAdmin.GetImage(imageId);
            image.Display = isDisplay;
            await _artistAdmin.UpdateImage(image);

            return Redirect("/Index?isActive=true");
        }

        /// <summary>
        /// Remove an image from the recent image gallery.
        /// </summary>
        public async Task<IActionResult> OnPostDeleteRecentImage(int arcanumMainId, int imageId)
        {
            await _artistAdmin.RemoveImageFromRecent(arcanumMainId, imageId);

            return Redirect("/Index?isActive=true");
        }
    }
}
