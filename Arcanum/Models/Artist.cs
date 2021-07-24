﻿using System;
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
        public int Order { get; set; }
        public bool Display { get; set; }
        public ArtistBooking ArtistBooking { get; set; }
        public ArtistPortfolio ArtistPortfolio { get; set; }
        public ArcanumMain ArcanumMain { get; set; }
    }
}
