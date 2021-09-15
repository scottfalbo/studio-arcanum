using Arcanum.Auth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Arcanum.Models.Interfaces
{
    public interface IWizardLord
    {
        public Task<List<string>> GetRoles();
        public Task<List<ApplicationUserDto>> GetRegisteredUsers();
    }
}
