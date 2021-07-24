using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Arcanum.Models
{
    public class Image
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Artist { get; set; }
        public string SourceUrl { get; set; }
        public string FileName { get; set; }
        public int Order { get; set; }
        public RecentImage RecentImage { get; set; }
        public Portfolio Portfolio { get; set; }

    }
}
