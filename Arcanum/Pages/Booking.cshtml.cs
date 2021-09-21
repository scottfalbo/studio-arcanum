using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Arcanum.Auth.Models;
using Arcanum.Models;
using Arcanum.Models.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Arcanum.Pages
{
    public class BookingModel : PageModel
    {
        private readonly IArtistAdmin _artistAdmin;
        private readonly ISite _siteAdmin;
        private readonly UserManager<ApplicationUser> _userManager;

        public BookingModel(IArtistAdmin artistAdmin, ISite siteAdmin, UserManager<ApplicationUser> userManager)
        {
            _artistAdmin = artistAdmin;
            _siteAdmin = siteAdmin;
            _userManager = userManager;
        }
        
        [BindProperty]
        public Booking Booking { get; set; }
        [BindProperty]
        public Artist Artist { get; set; }
        [BindProperty]
        public string UserId { get; set; }

        public async Task OnGet(string artistId)
        {
            await RefreshPage(artistId);
        }

        public async Task OnPostUpdate()
        {
            await _artistAdmin.UpdateArtistBooking(Booking);

            await RefreshPage(Artist.Id);
            Redirect($"/Booking?artistId={UserId}");
        }

        private async Task RefreshPage(string artistId)
        {
            if (artistId != null)
            {
                Booking = await _artistAdmin.GetArtistBooking(artistId);
                Artist = await _siteAdmin.GetArtist(artistId);
            }
            if (User.IsInRole("ArtistAdmin"))
            {
                var userName = User.Identity.Name;
                var user = await _userManager.FindByNameAsync(userName);
                UserId = user.Id;
            }
            else
                UserId = "";
        }
    }
}
