using Arcanum.Auth.Models;
using Arcanum.Models;
using Arcanum.Models.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Arcanum.Components
{
    [ViewComponent]
    public class ArtistLargeNavViewComponent : ViewComponent
    {
        public ISite _siteAdmin;

        public ArtistLargeNavViewComponent(ISite site)
        {
            _siteAdmin = site;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            List<Artist> artists = await _siteAdmin.GetArtists();

            ViewModel viewModel = new ViewModel()
            {
                Artists = artists.OrderBy(artist => artist.Order)
            };

            return View(viewModel);
        }

        public class ViewModel
        {
            public IEnumerable<Artist> Artists { get; set; }
        }
    }
}
