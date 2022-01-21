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
    public class ArtistNavViewComponent : ViewComponent
    {
        public ISite _siteAdmin;
        private readonly UserManager<ApplicationUser> _userManager;

        public ArtistNavViewComponent(ISite site, UserManager<ApplicationUser> userManager)
        {
            _siteAdmin = site;
            _userManager = userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            ApplicationUser user = null;
            if (User.IsInRole("ArtistAdmin"))
            {
                var userName = User.Identity.Name;
                user = await _userManager.FindByNameAsync(userName);
            }

            List<Artist> artists = await _siteAdmin.GetArtists();
            IEnumerable<Artist> artistList = artists.OrderBy(artist => artist.Order);

            ViewModel viewModel = new ViewModel()
            {
                Artists = artistList,
                UserId = user != null ? user.Id : "",
            };

            return View(viewModel);
        }

        public class ViewModel
        {
            public IEnumerable<Artist> Artists { get; set; }
            public string UserId { get; set; }
        }
    }
}
