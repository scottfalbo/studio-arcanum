using Arcanum.Models;
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

        public ArtistNavViewComponent()
        {

        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            List<Artist> artists = new List<Artist>();
            artists.Add(dummyData("spaceghost"));
            artists.Add(dummyData("harry"));
            artists.Add(dummyData("luci"));
            artists.Add(dummyData("ethel"));

            ViewModel viewModel = new ViewModel()
            {
                Artists = artists
            };

            return View(viewModel);
        }

        private Artist dummyData(string name) =>
            new Artist() { Name = name };

        public class ViewModel
        {
            public List<Artist> Artists { get; set; }
        }
    }



}
