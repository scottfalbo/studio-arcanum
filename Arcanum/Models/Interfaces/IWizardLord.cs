using Arcanum.Auth.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Arcanum.Models.Interfaces
{
    public interface IWizardLord
    {
        public IQueryable<IdentityRole> GetRoles();
        public Task<IdentityRole> GetRole(string id);
        public Task UpdateUserRoles(string userId, string[] roles);
        public Task<List<ApplicationUserDto>> GetRegisteredUsers();
        public Task DeleteUser(string userId);

        public Task<RegistrationAccessCode> CreateRegistrationAccessCode(string name);
        public Task<RegistrationAccessCode> GetRegistrationAccessCode(string code);
        public Task<List<RegistrationAccessCode>> GetRegistrationAccessCodes();
        public Task DeleteRegistrationAccessCode(string code);
    }
}
