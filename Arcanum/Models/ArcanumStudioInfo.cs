using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Arcanum.Models
{
    public class ArcanumStudioInfo
    {
        public int ArcanumId { get; set; }
        public int StudioInfoId { get; set; }
        public Arcanum Arcanum { get; set; }
        public StudioInfo StudioInfo { get; set; }
    }
}
