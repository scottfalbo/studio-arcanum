using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Arcanum.Models.Interfaces
{
    public interface IArtistAdmin
    {
        public Task<Portfolio> CreatePortfolio(string title);
        public Task AddPortfolioToArtist(string artistId, int portfolioId);


        public Task<Image> CreateImage(Image image);
        public Task AddImageToPortfolio(int portfolioId, int imageId);

    }
}
