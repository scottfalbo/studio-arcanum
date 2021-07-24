using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Arcanum.Models
{
    public class Booking
    {
        public int Id { get; set; }
        public string BookingInfo { get; set; }
        public string BookingEmail { get; set; }
        public ArtistBooking ArtistBooking { get; set; }
    }
}
