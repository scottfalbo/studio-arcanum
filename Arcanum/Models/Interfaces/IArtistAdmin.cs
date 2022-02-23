using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Arcanum.Models.Interfaces
{
    public interface IArtistAdmin
    {
        public Task<Portfolio> CreatePortfolio(string title);
        public Task<List<Portfolio>> GetPortfolios();
        public Task UpdatePortfolio(Portfolio portfolio);
        public Task AddPortfolioToArtist(string artistId, int portfolioId);
        public Task DeletePortfolio(int portfolioId, string artistId);
        public Task RemovePortfolioFromArtist(int portfolioId, string artistId);
        public Task<IEnumerable<PortfolioImage>> GetPortfolioImages(int id);

        public Task<Image> CreateImage(IFormFile file, string artistId);
        public Task<Image> GetImage(int imageId);
        public Task UpdateImage(Image image);
        public Task UpdateImageOrder(int imageId, int order);
        public Task AddImageToPortfolio(int portfolioId, int imageId);
        public Task RemoveImageFromPortfolio(int portfolioId, int imageId);
        public Task AddImageToRecent(int arcanumMainId, int imageId);
        public Task RemoveImageFromRecent(int arcanumMainId, int imageId);
        public Task<IEnumerable<RecentImage>> GetRecentImages(int arcanumMainId);
        public Task DeleteImage(int imageId);

        public Task<Booking> GetArtistBooking(string artistId);

        public Task<Artist> GetArtist(string id);

    }
}
