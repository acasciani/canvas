using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GreenLight.Models
{
    [Serializable]
    public class NeighborhoodSimple
    {
        public int NeighborhoodID { get; set; }
        public string Route { get; set; }
        public string Name { get; set; }
    }
}