using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Arcanum.Models;
using Arcanum.Models.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Arcanum.Pages
{
    public class ArtistModel : PageModel
    {
        public ISite _siteAdmin { get; set; }

        public ArtistModel(ISite siteAdmin)
        {
            _siteAdmin = siteAdmin;
        }

        public Artist Artist { get; set; }

        public async Task OnGet(string artistId)
        {
            Artist = await _siteAdmin.GetArtist(artistId);
        }
    }
}
