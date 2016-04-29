using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GreenLight
{
    [MetadataType(typeof(Neighborhood.Metadata))]
    public partial class Neighborhood
    {
        sealed class Metadata
        {
            [StringLength(200, MinimumLength = 3)]
            [Required]
            public string Name { get; set; }

            [StringLength(100, MinimumLength = 3)]
            [Required]
            public string URLName { get; set; }

            [StringLength(150, MinimumLength = 10)]
            [Required]
            public string AdminEmail { get; set; }

            [StringLength(20, MinimumLength = 4)]
            [Required]
            public string AdminPassPhrase { get; set; }

            [StringLength(20, MinimumLength = 4)]
            [Required]
            public string AdminUserName { get; set; }

            [Required]
            public decimal Latitude { get; set; }

            [Required]
            public decimal Longitude { get; set; }

            [Required]
            public int ZoomIndex { get; set; }
        }
    }
}