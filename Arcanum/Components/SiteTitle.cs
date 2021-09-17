using Arcanum.Models;
using Arcanum.Models.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Arcanum.Components
{
    [ViewComponent]
    public class SiteTitleViewComponent : ViewComponent
    {
        public ISite _siteAdmin;
        public SiteTitleViewComponent(ISite site)
        {
            _siteAdmin = site;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            ArcanumMain site = await _siteAdmin.GetMainPage();

            ViewModel viewModel = new ViewModel()
            {
                SiteTitle = site.SiteTitle
            };

            return View(viewModel);
        }

        public class ViewModel
        {
            public string SiteTitle { get; set; }
        }
    }
}
