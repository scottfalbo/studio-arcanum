using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Arcanum.Models
{
    public class RecentImage
    {
        public int ArcanumMainId { get; set; }
        public int ImageId { get; set; }
        public DateTime DateTime { get; set; }
        public ArcanumMain ArcanumMain { get; set; }
        public Image Image { get; set; }
    }
}
