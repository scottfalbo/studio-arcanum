using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Arcanum.Models;
using Arcanum.Models.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Arcanum.Pages
{
    public class TheStudioModel : PageModel
    {
        private readonly ISite _siteAdmin;
        private readonly IArtistAdmin _artistAdmin;

        public TheStudioModel(ISite siteAdmin, IArtistAdmin artistAdmin)
        {
            _siteAdmin = siteAdmin;
            _artistAdmin = artistAdmin;
        }

        [BindProperty]
        public StudioInfo StudioInfo { get; set; }
        public bool ActiveAdmin { get; set; }

        public async Task OnGet(bool isActive = false)
        {
            StudioInfo = await _siteAdmin.GetStudio();
            ActiveAdmin = isActive;
        }

        public async Task<IActionResult> OnPostUpdate()
        {
            await _siteAdmin.UpdateStudioInfo(StudioInfo);
            await _siteAdmin.UpdateStudioAddress(StudioInfo.Address);

            return Redirect("/TheStudio?isActive=true");
        }

        public async Task<IActionResult> OnPostAddImages(IFormFile[] files)
        {
            foreach (IFormFile file in files)
            {
                Image image = await _siteAdmin.UpdateMainPageImage(file);
                await _siteAdmin.AddImageToStudio(StudioInfo.Id, image.Id);
            }

            return Redirect("/TheStudio?isActive=true");
        }

        public async Task<IActionResult> OnPostDeleteImage(int imageId)
        {
            await _siteAdmin.RemoveImageFromStudio(StudioInfo.Id, imageId);
            await _artistAdmin.DeleteImage(imageId);

            return Redirect("/TheStudio?isActive=true");
        }
    }
}
