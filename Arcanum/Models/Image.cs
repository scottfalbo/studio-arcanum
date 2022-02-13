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
        public string AltText { get; set; }
        public string ArtistId { get; set; }
        public string SourceUrl { get; set; }
        public string ThumbnailUrl { get; set; }
        public string FileName { get; set; }
        public string ThumbFileName { get; set; }
        public bool Display { get; set; }
        public List<RecentImage> RecentImage { get; set; }
        public List<PortfolioImage> PortfolioImage { get; set; }
        public List<StudioImage> StudioImages { get; set; }
    }
}
