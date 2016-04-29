using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GreenLight.Controllers
{
    public class LoadNeighborhoodController : Controller
    {
        // GET: LoadNeighborhood
        public ActionResult Index(string neighborhood)
        {
            int _id;
            int neighborhoodID = int.TryParse(neighborhood, out _id) ? _id : CacheManager.GetNeighborhoodID(neighborhood).Value;

            Session["NeighborhoodID"] = neighborhoodID;

            return RedirectToAction("Index", "Home");
        }
    }
}