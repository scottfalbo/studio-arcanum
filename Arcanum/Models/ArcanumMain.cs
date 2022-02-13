using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Arcanum.Models
{
    public class ArcanumMain
    {
        public int Id { get; set; }
        public string SiteTitle { get; set; }
        public string IntroA { get; set; }
        public string IntroB { get; set; }
        public PageImage ImageOne { get; set; }
        public PageImage ImageTwo { get; set; }
        public PageImage ImageThree { get; set; }
        public List<RecentImage> RecentImage { get; set; }
    }
}
