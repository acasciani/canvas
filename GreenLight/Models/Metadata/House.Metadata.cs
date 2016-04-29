using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GreenLight
{
    [MetadataType(typeof(House.Metadata))]
    public partial class House
    {
        sealed class Metadata
        {
            [StringLength(50, MinimumLength = 4)]
            [Required]
            public string Address { get; set; }

            public Nullable<decimal> Latitude { get; set; }
            public Nullable<decimal> Longitude { get; set; }

            public int NeighborhoodID { get; set; }

            [StringLength(50)]
            [Required]
            public string City { get; set; }

            [StringLength(10)]
            [Required]
            public string ZipCode { get; set; }
        }
    }
}