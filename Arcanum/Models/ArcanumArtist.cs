using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Arcanum.Models
{
    public class ArcanumArtist
    {
        public int ArcanumId { get; set; }
        public string ArtistId { get; set; }
        public Arcanum Arcanum { get; set; }
        public Artist Artist { get; set; }
    }
}
