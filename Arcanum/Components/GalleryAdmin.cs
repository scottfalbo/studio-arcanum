using Arcanum.Models.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Arcanum.Components
{
    [ViewComponent]
    public class GalleryAdminViewComponent : ViewComponent
    {
        public IArtistAdmin _artistAdmin;

        public GalleryAdminViewComponent(IArtistAdmin artistAdmin)
        {
            _artistAdmin = artistAdmin;
        }

        public async Task<IViewComponentResult> InvokeAsync(int portfolioId)
        {
            ViewModel viewModel = new ViewModel
            {
                PortfolioId = portfolioId
            };
            return View(viewModel);
        }

        public class ViewModel
        {
            public int PortfolioId { get; set; }
        }
    }
}
