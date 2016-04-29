using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;

namespace GreenLight.App_Start
{
    public class NeighborhoodNameConstraint : IRouteConstraint
    {
        public bool Match(HttpContextBase httpContext, Route route, string parameterName, RouteValueDictionary values, RouteDirection routeDirection)
        {
            int? neighborhoodID = CacheManager.GetNeighborhoodID(values[parameterName].ToString());
            return neighborhoodID.HasValue ? true : false;
        }
    }
}