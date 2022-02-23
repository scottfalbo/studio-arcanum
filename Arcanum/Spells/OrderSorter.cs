using Arcanum.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Arcanum.Spells
{
    public class OrderSorter
    {
        public static Artist SortPortfolioImages(Artist artist)
        {
            for(int i = 0; i < artist.ArtistPortfolios.Count; i++)
            {
                var images = artist.ArtistPortfolios[i].Portfolio.PortfolioImage
                    .OrderBy(x => x.Order).ToList();
                artist.ArtistPortfolios[i].Portfolio.PortfolioImage = images;   
            }

            return artist;
        }
    }
}
