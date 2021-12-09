using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Arcanum.Models;
using Arcanum.Models.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Arcanum.Pages
{
    public class TheStudioModel : PageModel
    {
        private readonly ISite _siteAdmin;

        public TheStudioModel(ISite siteAdmin)
        {
            _siteAdmin = siteAdmin;
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

            return Redirect("/TheStudio?isActive=true");
        }
    }
}
