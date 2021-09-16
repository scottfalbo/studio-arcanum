using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Arcanum.Models
{
    public class Artist
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Intro { get; set; }
        public string Instagram { get; set; }
        public string ProfileImageUri { get; set; }
        public string ProfileImageFileName { get; set; }
        public int Order { get; set; }
        public bool Display { get; set; }
        public ArtistBooking ArtistBooking { get; set; }
        public List<ArtistPortfolio> ArtistPortfolios { get; set; }
    }
}
