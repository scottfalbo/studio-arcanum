using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Arcanum.Models
{
    public class Address
    {
        [ForeignKey("StudioInfo")]
        public int AddressId { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public int ZipCode { get; set; }
        public StudioInfo StudioInfo { get; set; }
    }
}
