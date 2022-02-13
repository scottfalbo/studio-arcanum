using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Arcanum.Models
{
    public class PageImage
    {
        public int ArcanumMainId { get; set; }
        public int ImageId { get; set; }
        public ArcanumMain ArcanumMain { get; set; }
        public Image Image { get; set; }
    }
}
