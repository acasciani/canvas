using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GreenLight
{
    [MetadataType(typeof(Visit.Metadata))]
    public partial class Visit
    {
        sealed class Metadata
        {
            [Required]
            public int HouseID { get; set; }
            
            [Required]
            public int ResponseID { get; set; }

            [StringLength(200)]
            public string Notes { get; set; }
        }
    }
}