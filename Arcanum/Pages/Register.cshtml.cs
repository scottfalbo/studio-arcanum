using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Arcanum.Auth.Models;
using Arcanum.Auth.Models.Interfaces;
using Arcanum.Models;
using Arcanum.Models.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Arcanum.Pages
{
    public class RegisterModel : PageModel
    {
        public IUserService _userService;
        public ISite _siteAdmin;
        public IArtistAdmin _artistAdmin;

        public RegisterModel(IUserService userService, ISite siteAdmin, IArtistAdmin artistAdmin)
        {
            _userService = userService;
            _siteAdmin = siteAdmin;
            _artistAdmin = artistAdmin;
        }
        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync(string username, string password, string email)
        {
            username = Regex.Replace(username, " ", "");
            RegisterUser newUser = new RegisterUser()
            {
                UserName = username,
                Password = password,
                Email = email
            };

            ApplicationUserDto user = await _userService.Register(newUser, this.ModelState);
            Artist artist = await _siteAdmin.CreateArtist(NewArtist(user));
            await AddDefaultPortfolio(artist.Id);

            return Redirect("/Login");
        }

        /// <summary>
        /// Helper method to add a default portfolio with placeholder images to a new artist profile.
        /// </summary>
        /// <param name="artistId"> string artistId </param>
        private async Task AddDefaultPortfolio(string artistId)
        {
            Portfolio portfolio = await _artistAdmin.CreatePortfolio("Gallery One");
            await _artistAdmin.AddPortfolioToArtist(artistId, portfolio.Id);
            List<Image> images = await CreateDefaultImages();

            foreach (Image image in images)
                await _artistAdmin.AddImageToPortfolio(portfolio.Id, image.Id);
        }

        /// <summary>
        /// Helper method to create a list of default placeholder images.
        /// </summary>
        /// <returns> List<Image> image objects </returns>
        private async Task<List<Image>> CreateDefaultImages()
        {
            List<Image> images = new List<Image>();
            for (int i = 0; i < 10; i++)
            {
                Image image = new Image()
                {
                    Title = $"default-image-{i}",
                    Artist = "unknown",
                    SourceUrl = "https://via.placeholder.com/60",
                    ThumbnailUrl = "https://via.placeholder.com/60"
                };
                images.Add(await _artistAdmin.CreateImage(image));
            }
            return images;
        }

        /// <summary>
        /// Helper method to instantiate a new Arist object.
        /// </summary>
        /// <param name="user"> ApplicationUserDto newUser </param>
        /// <returns> new Artist object </returns>
        private Artist NewArtist(ApplicationUserDto user)
        {
            return new Artist()
            {
                Id = user.Id,
                Name = user.UserName,
                Email = user.Email
            };
        }
    }
}
