using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Arcanum.Models
{
    public class ArcanumMain
    {
        public int Id { get; set; }
        public string HomePageIntro { get; set; }
        public List<RecentImage> Recent { get; set; }
        public List<ArcanumArtist> Artists { get; set; }
    }
}
