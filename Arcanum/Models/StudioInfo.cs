using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Arcanum.Models
{
    public class StudioInfo
    {
        public int Id { get; set; }
        public string Instagram { get; set; }
        public string Intro { get; set; }
        public string Email { get; set; }
        public Address Address { get; set; }
        public string Policies { get; set; }
        public string Aftercare { get; set; }
        public int ImageCount { get; set; }
        public List<StudioImage> StudioImages { get; set; }
    }
}
