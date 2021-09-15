using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Arcanum.Models.Interfaces
{
    public interface ISite
    {
        public Task CreateArtist(Artist artist);
        public Task<List<Artist>> GetArtists();
    }
}
