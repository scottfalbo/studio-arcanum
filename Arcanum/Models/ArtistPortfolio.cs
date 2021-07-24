﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Arcanum.Models
{
    public class ArtistPortfolio
    {
        public string ArtistId { get; set; }
        public int BookingId { get; set; }
        public Artist Artist { get; set; }
        public Portfolio Portfolio { get; set; }
    }
}
