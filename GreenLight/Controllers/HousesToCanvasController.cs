using Geocoding;
using Geocoding.Google;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GreenLight.Controllers
{
    public class HousesToCanvasController : Controller
    {
        private GreenLightEntities db = new GreenLightEntities();

        // GET: HousesToCanvas
        public ActionResult Index()
        {
            ViewBag.ResponseID = new SelectList(db.Responses.OrderBy(i => i.SortOrder), "ResponseID", "Name");
            return View();
        }

        [HttpGet]
        public JsonResult HousesLeftToCanvas()
        {
            List<int> housesWithResponses = db.Visits.Select(i=>i.HouseID).Distinct().ToList();
            var housesWithNoResponses = db.Houses.Where(i => !housesWithResponses.Contains(i.HouseID));

            return Json(housesWithNoResponses, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public void Visited(List<string> val)
        {
            int houseID = int.Parse(val[0]);
            int responseID = int.Parse(val[1]);
            string notes = val[2];

            db.Visits.Add(new Visit()
            {
                CreateDate = DateTime.Now,
                HouseID = houseID,
                Notes = notes,
                ResponseID = responseID
            });
            db.SaveChanges();
        }

        [HttpGet]
        public void GeoCode()
        {
            IGeocoder geocoder = new GoogleGeocoder() { ApiKey = "AIzaSyB7Mcq6kcU87hlj3mi8AosJ1R20YETpvjk" };

            IEnumerable<House> housesToUpdate = db.Houses.Where(i => !i.Latitude.HasValue || !i.Longitude.HasValue);

            foreach (House house in housesToUpdate)
            {
                Address address = geocoder.Geocode(string.Format("{0}, Rochester, NY 14610", house.Address)).FirstOrDefault();

                if (address == null)
                {
                    continue;
                }

                house.Latitude = Convert.ToDecimal(address.Coordinates.Latitude);
                house.Longitude = Convert.ToDecimal(address.Coordinates.Longitude);
            }

            db.SaveChanges();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}