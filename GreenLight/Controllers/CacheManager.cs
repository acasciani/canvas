using GreenLight.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GreenLight
{
    public class CacheManager
    {
        public static int? GetNeighborhoodID(string routeValue)
        {
            routeValue = routeValue.ToUpper();

            Dictionary<string, int> neighborhoodRoutes = HttpContext.Current.Cache.Get("NeighborhoodRoutes") as Dictionary<string, int>;

            if (neighborhoodRoutes == null)
            {
                neighborhoodRoutes = new Dictionary<string, int>();

                using (GreenLightEntities db = new GreenLightEntities())
                {
                    var _neighborhoods = db.Neighborhoods.Select(i => new NeighborhoodSimple()
                    {
                         NeighborhoodID = i.NeighborhoodID,
                          Name = i.Name,
                           Route = i.URLName
                    }).Distinct().ToList();
                    _neighborhoods.ForEach(i => neighborhoodRoutes.Add(i.Route.ToUpper(), i.NeighborhoodID));
                }

                HttpContext.Current.Cache.Insert("NeighborhoodRoutes", neighborhoodRoutes, null, DateTime.UtcNow.AddHours(1), System.Web.Caching.Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.AboveNormal, null);
            }

            return neighborhoodRoutes.ContainsKey(routeValue) ? neighborhoodRoutes[routeValue] : (int?)null;
        }


        public static NeighborhoodSimple GetNeighborhood(int neighborhoodID)
        {
            Dictionary<int, NeighborhoodSimple> neighborhoods = HttpContext.Current.Cache.Get("Neighborhoods") as Dictionary<int, NeighborhoodSimple>;

            if (neighborhoods == null)
            {
                neighborhoods = new Dictionary<int, NeighborhoodSimple>();

                using (GreenLightEntities db = new GreenLightEntities())
                {
                    var _neighborhoods = db.Neighborhoods.Select(i => new NeighborhoodSimple()
                    {
                        NeighborhoodID = i.NeighborhoodID,
                        Name = i.Name,
                        Route = i.URLName
                    }).Distinct().ToList();
                    _neighborhoods.ForEach(i => neighborhoods.Add(i.NeighborhoodID, i));
                }

                HttpContext.Current.Cache.Insert("Neighborhoods", neighborhoods, null, DateTime.UtcNow.AddHours(1), System.Web.Caching.Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.AboveNormal, null);
            }

            return neighborhoods.ContainsKey(neighborhoodID) ? neighborhoods[neighborhoodID] : null;
        }
    }
}