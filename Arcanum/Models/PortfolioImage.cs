using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Arcanum.Models
{
    public class PortfolioImage
    {
        public int PortfolioId { get; set; }
        public int ImageId { get; set; }
        public Portfolio Portfolio { get; set; }
        public Image Image { get; set; }
    }
}
