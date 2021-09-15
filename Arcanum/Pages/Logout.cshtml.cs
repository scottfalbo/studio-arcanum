using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Arcanum.Auth.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Arcanum.Pages
{
    public class LogoutModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signout;

        public LogoutModel(SignInManager<ApplicationUser> signout)
        {
            _signout = signout;
        }
        public async Task<IActionResult> OnGet()
        {
            await _signout.SignOutAsync();
            return Redirect("/Index");
        }
    }
}
