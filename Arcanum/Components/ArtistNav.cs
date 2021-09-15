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
    public class ArtistNavViewComponent : ViewComponent
    {
        public ISite _siteAdmin;
        public ArtistNavViewComponent(ISite site)
        {
            _siteAdmin = site;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            List<Artist> artists = await _siteAdmin.GetArtists();

            ViewModel viewModel = new ViewModel()
            {
                Artists = artists
            };

            return View(viewModel);
        }

        public class ViewModel
        {
            public List<Artist> Artists { get; set; }
        }
    }



}
