using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Arcanum.Models
{
    public class Artist
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public int Order { get; set; }
        public bool Display { get; set; }
        public ArtistBooking Booking { get; set; }
        public ArtistPortfolio Portfolio { get; set; }
        public ArcanumArtist ArcanumArtist { get; set; }
    }
}
