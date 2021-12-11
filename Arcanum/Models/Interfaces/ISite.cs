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


        public Task<Booking> CreateBooking();
        public Task AddBookingToArtist(string artistId, int bookingId);
        public Task DeleteBooking(int id);
        public Task RemoveBookingFromArtist(string artistId, int bookingId);

        public Task<ArcanumMain> GetMainPage();
        public Task UpdateMainPage(ArcanumMain mainPage);

        public Task<StudioInfo> GetStudio();
        public Task UpdateStudioInfo(StudioInfo studioInfo);
    }
}
