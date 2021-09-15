using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Arcanum.Models
{
    public class Portfolio
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Intro { get; set; }
        public string Instagram { get; set; }
        public List<PortfolioImage> PortfolioImage { get; set; }
        public List<ArtistPortfolio> ArtistPortfolio { get; set; }
    }
}
