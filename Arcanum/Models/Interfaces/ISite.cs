using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Arcanum.Models.Interfaces
{
    public interface ISite
    {
        public Task<Artist> CreateArtist(Artist artist);
        public Task<Artist> GetArtist(string id);
        public Task<List<Artist>> GetArtists();
        public Task UpdateArtist(Artist artist);
        public Task DeleteArtist(string id);

        public Task<ArcanumMain> GetMainPage();
        public Task UpdateMainPage(ArcanumMain mainPage);
    }
}
