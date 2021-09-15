using Arcanum.Auth.Models;
using Arcanum.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Arcanum.Models.Interfaces.Services
{
    public class WizardLordService : IWizardLord
    {
        private readonly ArcanumDbContext _db;

        public WizardLordService(ArcanumDbContext context)
        {
            _db = context;
        }

        public Task<List<ApplicationUserDto>> GetRegisteredUsers()
        {
            throw new NotImplementedException();
        }

        public Task<List<string>> GetRoles()
        {
            throw new NotImplementedException();
        }
    }
}
