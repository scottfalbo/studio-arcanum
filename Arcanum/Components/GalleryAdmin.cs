using Arcanum.Data;
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
    public class GalleryAdminViewComponent : ViewComponent
    {
        private readonly ArcanumDbContext _db;
        public IArtistAdmin _artistAdmin;

        public GalleryAdminViewComponent(IArtistAdmin artistAdmin, ArcanumDbContext db)
        {
            _artistAdmin = artistAdmin;
            _db = db;
        }

        public async Task<IViewComponentResult> InvokeAsync(int portfolioId)
        {
            List<PortfolioImage> portfolioImages = await _artistAdmin.GetPortfolioImages(portfolioId);
            List<Image> images = new List<Image>();
            foreach(PortfolioImage image in portfolioImages)
            {
                images.Add(await _db.Image.FindAsync(image.ImageId));
            }

            ViewModel viewModel = new ViewModel
            {
                PortfolioId = portfolioId,
                Images = images
            };
            return View(viewModel);
        }

        public class ViewModel
        {
            public int PortfolioId { get; set; }
            public List<Image> Images { get; set; }
        }
    }
}
