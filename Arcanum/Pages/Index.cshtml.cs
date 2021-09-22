using Arcanum.Models;
using Arcanum.Models.Interfaces;
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

        public IndexModel(ILogger<IndexModel> logger, ISite siteAdmin)
        {
            _logger = logger;
            _siteAdmin = siteAdmin;
        }

        [BindProperty]
        public ArcanumMain MainPage { get; set; }
        public string Instagram { get; set; }

        public async Task OnGet()
        {
            await RefreshPage();
        }

        /// <summary>
        /// Update the main page text area
        /// </summary>
        public async Task OnPostUpdate()
        {
            await _siteAdmin.UpdateMainPage(MainPage);

            await RefreshPage();
            Redirect("/");
        }

        /// <summary>
        /// Helper method to refresh the page properties when refreshed without an IActionResult.
        /// </summary>
        private async Task RefreshPage()
        {
            Instagram = (await _siteAdmin.GetStudio()).Instagram;
            MainPage = await _siteAdmin.GetMainPage();
        }
    }
}
