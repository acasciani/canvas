using GreenLight.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GreenLight.Controllers
{
    public class HomeController : Controller
    {
        [Serializable]
        public class ChartResult
        {
            public int Count { get; set; }
            public string IndexLabel { get; set; }
            public string Legend { get; set; }
        }


        GreenLightEntities db = new GreenLightEntities();

        public ActionResult Index()
        {
            NeighborhoodSimple neighborhood = CacheManager.GetNeighborhood(int.Parse((Session["NeighborhoodID"] ?? "0").ToString()));

            if (neighborhood == null)
            {
                return RedirectToAction("About", "Home");
            }

            List<int> houseIDsInNeighborhood = db.Houses.Where(i => i.NeighborhoodID == neighborhood.NeighborhoodID).Select(i => i.HouseID).ToList();

            var allResponsesGroupedByHouse = db.Visits.Where(i => houseIDsInNeighborhood.Contains(i.HouseID)).GroupBy(i => i.HouseID)
                .Select(i => i.OrderByDescending(j => j.CreateDate).FirstOrDefault())
                .GroupBy(i => i.ResponseID)
                .Select(i => new ChartResult()
                {
                    Legend = i.FirstOrDefault().Respons.Name,
                    IndexLabel = i.FirstOrDefault().Respons.Name,
                    Count = i.Count()
                })
                .ToList();

            var houseIDsCanvassed = db.Visits.Where(i => houseIDsInNeighborhood.Contains(i.HouseID)).Select(i => i.HouseID).Distinct().ToList();
            var housesNotYetCanvassed = db.Houses.Where(i => houseIDsInNeighborhood.Contains(i.HouseID) && !houseIDsCanvassed.Contains(i.HouseID)).Select(i => i.HouseID).Distinct().ToList();

            List<ChartResult> housesNotYetCanvassedData = new List<ChartResult>();
            housesNotYetCanvassedData.Add(new ChartResult()
            {
                Count = housesNotYetCanvassed.Count(),
                IndexLabel = "Not Yet Canvassed",
                Legend = "Not Yet Canvassed"
            });
            housesNotYetCanvassedData.Add(new ChartResult()
            {
                Count = houseIDsCanvassed.Count(),
                IndexLabel = "Already Canvassed",
                Legend = "Already Canvassed"
            });

            List<int> housesCommitted = db.Visits.Where(i => houseIDsInNeighborhood.Contains(i.HouseID) && (i.ResponseID == 1 || i.ResponseID == 2)).Select(i => i.HouseID).Distinct().ToList();
            List<int> housesNotCommitted = db.Houses.Where(i => houseIDsInNeighborhood.Contains(i.HouseID) && !housesCommitted.Contains(i.HouseID)).Select(i=>i.HouseID).Distinct().ToList();

            List<ChartResult> housesNotYetCommittedData = new List<ChartResult>();
            housesNotYetCommittedData.Add(new ChartResult()
            {
                Count = housesCommitted.Count(),
                IndexLabel = "Houses Committed",
                Legend = "Houses Committed (Signed up or WILL sign up)"
            });
            housesNotYetCommittedData.Add(new ChartResult()
            {
                Count = housesNotCommitted.Count(),
                IndexLabel = "Houses Not Committed",
                Legend = "Houses Not Committed"
            });

            ViewBag.LatestResponsesReceived = allResponsesGroupedByHouse;
            ViewBag.HousesCanvassedStats = housesNotYetCanvassedData;
            ViewBag.HousesNotYetCommitted = housesNotYetCommittedData;
            ViewBag.Neighborhood = neighborhood;

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}