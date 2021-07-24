using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Arcanum.Auth.Models.Interfaces.Services
{
    public class IdentityUserService : IUserService
    {
        public Task<ApplicationUserDto> Authenticate(string userName, string password)
        {
            throw new NotImplementedException();
        }

        public Task<ApplicationUserDto> Register(RegisterUser data, ModelStateDictionary modelState)
        {
            throw new NotImplementedException();
        }
    }
}

