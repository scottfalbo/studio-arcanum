using Arcanum.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Arcanum.ImageBlob.Interfaces
{
    public interface IUpload
    {
        public Task<Image> AddImage(IFormFile file);
        public Task<Image> UpdateSiteImage(IFormFile file);
        public Task<Image> UpdateProfilePhoto(IFormFile file);
        public Task RemoveImage(string fileName);
    }
}
