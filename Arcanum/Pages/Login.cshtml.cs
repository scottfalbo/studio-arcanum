using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Arcanum.Auth.Models;
using Arcanum.Auth.Models.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Arcanum.Pages
{
    public class LoginModel : PageModel
    {
        public IUserService _userService { get; }
        public LoginModel(IUserService userService)
        {
            _userService = userService;
        }

        public bool InValid { get; set; }

        public void OnGet(bool invalid = false)
        {
            InValid = invalid;
        }

        public async Task<IActionResult> OnPostAsync(string username, string password)
        {
            var user = await _userService.Authenticate(username, password);
            if (user != null)
                return Redirect($"/Artist?artistId={user.Id}");
            return Redirect("/Login?invalid=true");
        }
    }
}
