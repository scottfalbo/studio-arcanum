﻿using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Arcanum.Auth.Models.Interfaces
{
    public interface IUserService
    {
        Task<ApplicationUserDto> Register(RegisterUser data, ModelStateDictionary modelState);

        Task<ApplicationUserDto> Authenticate(string userName, string password);
    }
}
