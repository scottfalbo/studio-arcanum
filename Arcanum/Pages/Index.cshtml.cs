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
        private readonly ISite _site;

        public IndexModel(ILogger<IndexModel> logger, ISite siteAdmin)
        {
            _logger = logger;
            _site = siteAdmin;
        }

        [BindProperty]
        public ArcanumMain MainPage { get; set; }
        public string Instagram { get; set; }
        public bool ActiveAdmin { get; set; }

        public async Task OnGet(bool isActive = false)
        {
            Instagram = (await _site.GetStudio()).Instagram;
            MainPage = await _site.GetMainPage();
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
    }
}
