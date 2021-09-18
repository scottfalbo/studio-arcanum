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

        public StudioInfo StudioInfo { get; set; }

        public async Task OnGet()
        {
            StudioInfo = await _siteAdmin.GetStudio();
        }
    }
}
