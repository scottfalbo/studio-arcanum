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
        private readonly ISite _site;
        private readonly IArtistAdmin _artistAdmin;

        public IndexModel(ILogger<IndexModel> logger, ISite siteAdmin, IArtistAdmin artistAdmin)
        {
            _logger = logger;
            _site = siteAdmin;
            _artistAdmin = artistAdmin;
        }

        [BindProperty]
        public ArcanumMain MainPage { get; set; }
        public Image[] PageImages { get; set; }
        public bool ActiveAdmin { get; set; }
        [BindProperty]
        public IEnumerable<Artist> Artists { get; set; }
        [BindProperty]
        public StudioInfo StudioInfo { get; set; }

        public async Task OnGet(bool isActive = false)
        {
            MainPage = await _site.GetMainPage();
            PageImages = new Image[3];
            PageImages[0].SourceUrl = MainPage.ImageOneSourceUrl;
            PageImages[1].SourceUrl = MainPage.ImageTwoSourceUrl;
            PageImages[2].SourceUrl = MainPage.ImageThreeSourceUrl;
            Artists = await _site.GetArtists();
            StudioInfo = await _site.GetStudio();
            ActiveAdmin = isActive;
        }

        /// <summary>
        /// Update the main page text area
        /// </summary>
        public async Task<IActionResult> OnPostUpdate()
        {
            await _site.UpdateMainPage(MainPage);

            return Redirect("/Index?isActive=true");
        }

        /// <summary>
        /// Updates the images on the main page.
        /// </summary>
        public async Task<IActionResult> OnPostUpdatePageImage(IFormFile file, int index)
        {
            Image image = await _site.UpdateMainPageImage(file);
            ArcanumMain arcanumMain = await _site.GetMainPage();
            switch (index)
            {
                case 0:
                    arcanumMain.ImageOneSourceUrl = image.SourceUrl;
                    break;
                case 1:
                    arcanumMain.ImageTwoSourceUrl = image.SourceUrl;
                    break;
                case 2:
                    arcanumMain.ImageThreeSourceUrl = image.SourceUrl;
                    break;
                default:
                    break;
            }
            await _site.UpdateMainPage(arcanumMain);

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
