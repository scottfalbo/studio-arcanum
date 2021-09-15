using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Arcanum.Data;
using Arcanum.Models.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Arcanum.Pages.Admin
{
    public class SecretLairModel : PageModel
    {
        private readonly ArcanumDbContext _db;
        public ISite _siteAdmin;
        public IWizardLord _wizard;

        public SecretLairModel(ArcanumDbContext db, ISite siteAdmin, IWizardLord magicPower)
        {
            _db = db;
            _siteAdmin = siteAdmin;
            _wizard = magicPower;
        }



        public void OnGet()
        {
        }
    }
}
