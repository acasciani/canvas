using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GreenLight
{
    [MetadataType(typeof(Option.Metadata))]
    public partial class Option
    {
        sealed class Metadata
        {
            [StringLength(50, MinimumLength = 3)]
            [Required]
            public string Value { get; set; }

            [Required]
            public int NeighborhoodID { get; set; }
            
            [Required]
            public int OptionTypeID { get; set; }
        }
    }
}