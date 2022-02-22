using Arcanum.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Arcanum.Spells
{
    public class BootStrapAccordionIds
    {
        /// <summary>
        /// Properties for portfolio bootstrap accordion.
        /// </summary>
        /// <param name="portfolio"> portfolio </param>
        /// <returns> portfolio w/ids </returns>
        public static Portfolio PortfolioAccordionIds(Portfolio portfolio)
        {
            string str = (Regex.Replace(portfolio.Title, @"\s+", String.Empty)).ToLower();

            portfolio.AccordionId = $"accordion-id-{portfolio.Id}-{str}";
            portfolio.CollapseId = $"collapse-id-{portfolio.Id}-{str}";
            portfolio.AdminAccordionId = $"admin-accordion-id-{portfolio.Id}-{str}";
            portfolio.AdminCollapseId = $"admin-collapse-id-{portfolio.Id}-{str}";

            return portfolio;
        }

        /// <summary>
        /// Properties for artist bootstrap accordion.
        /// </summary>
        /// <param name="artist"> artist </param>
        /// <returns> artist w/ids </returns>
        public static Artist ArtistAccordionIds(Artist artist)
        {
            string str = (Regex.Replace(artist.Name, @"\s+", String.Empty)).ToLower();

            artist.AccordionId = $"accordion-id-{artist.Id}-{str}";
            artist.CollapseId = $"collapse-id-{artist.Id}-{str}";
            artist.AdminAccordionId = $"admin-accordion-id-{artist.Id}-{str}";
            artist.AdminCollapseId = $"admin-collapse-id-{artist.Id}-{str}";

            return artist;
        }
    }
}
