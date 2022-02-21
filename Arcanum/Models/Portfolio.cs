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
        public bool Display { get; set; }
        public int Order { get; set; }
        public int ImageCount { get; set; }

        /// <summary>
        /// unique class names for the bootstrap accordion
        /// </summary>
        public string AccordionId { get; set; }
        public string CollapseId { get; set; }
        public string AdminAccordionId { get; set; }
        public string AdminCollapseId { get; set; }

        public List<PortfolioImage> PortfolioImage { get; set; }
        public List<ArtistPortfolio> ArtistPortfolio { get; set; }
    }
}
