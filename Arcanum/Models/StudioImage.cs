using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Arcanum.Models
{
    public class StudioImage
    {
        public int StudioInfoId { get; set; }
        public int ImageId { get; set; }
        public int Order { get; set; }
        public StudioInfo StudioInfo { get; set; }
        public Image Image { get; set; }
    }
}
