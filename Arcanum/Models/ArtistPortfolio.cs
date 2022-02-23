using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Arcanum.Models
{
    public class ArtistPortfolio
    {
        public string ArtistId { get; set; }
        public int PortfolioId { get; set; }
        public int Order { get; set; }
        public Artist Artist { get; set; }
        public Portfolio Portfolio { get; set; }
    }
}
