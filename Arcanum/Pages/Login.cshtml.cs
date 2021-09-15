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

        public async Task<IActionResult> OnPostAsync(string username, string password)
        {
            var user = await _userService.Authenticate(username, password);
            return Redirect("/Index");
        }
    }
}
