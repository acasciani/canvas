﻿using Geocoding;
using Geocoding.Google;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GreenLight.Controllers
{
    [AllowAnonymous]
    public class HousesToCanvasController : Controller
    {
        private GreenLightEntities db = new GreenLightEntities();

        private int? NeighborhoodID { get; set; }

        public HousesToCanvasController()
        {
            int _id;
            if (int.TryParse((System.Web.HttpContext.Current.Session["NeighborhoodID"] ?? "").ToString(), out _id))
            {
                NeighborhoodID = _id;
            }
        }

        // GET: HousesToCanvas
        public ActionResult Index()
        {
            if (!NeighborhoodID.HasValue)
            {
                return RedirectToAction("Welcome", "Home");
            }

            ViewBag.ResponseID = new SelectList(db.Responses.OrderBy(i => i.SortOrder), "ResponseID", "Name");
            ViewBag.Neighborhood = CacheManager.GetNeighborhood(NeighborhoodID.Value);

            var mapSettings = db.Neighborhoods.Where(i => i.NeighborhoodID == NeighborhoodID.Value).Select(i => new
            {
                CenterLatitude = i.Latitude,
                CenterLongitude = i.Longitude,
                ZoomIndex = i.ZoomIndex
            }).First();

            ViewBag.CenterLatitude = mapSettings.CenterLatitude;
            ViewBag.CenterLongitude = mapSettings.CenterLongitude;
            ViewBag.ZoomIndex = mapSettings.ZoomIndex;
            

            return View();
        }

        [HttpGet]
        public JsonResult HousesLeftToCanvas()
        {
            if (!NeighborhoodID.HasValue)
            {
                return Json(new object(), JsonRequestBehavior.AllowGet);
            }

            List<int> housesWithResponses = db.Visits.Where(i => i.House.NeighborhoodID == NeighborhoodID.Value).Select(i => i.HouseID).Distinct().ToList();
            var housesWithNoResponses = db.Houses.Where(i=>i.NeighborhoodID == NeighborhoodID.Value && i.Latitude.HasValue && i.Longitude.HasValue).Where(i => !housesWithResponses.Contains(i.HouseID))
                .Select(i => new
                {
                    Address = i.Address,
                    HouseID = i.HouseID,
                    Latitude = i.Latitude,
                    Longitude = i.Longitude
                }).ToList();

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
                CreateDate = DateTime.Now.ToUniversalTime(),
                HouseID = houseID,
                Notes = notes,
                ResponseID = responseID
            });
            db.SaveChanges();
        }

        [HttpGet]
        public ActionResult GeoCode()
        {
            IGeocoder geocoder = new GoogleGeocoder() { ApiKey = "AIzaSyB7Mcq6kcU87hlj3mi8AosJ1R20YETpvjk" };

            IEnumerable<House> housesToUpdate = db.Houses.Where(i => i.NeighborhoodID == NeighborhoodID && (!i.Latitude.HasValue || !i.Longitude.HasValue));

            foreach (House house in housesToUpdate)
            {
                Address address = geocoder.Geocode(string.Format("{0}, {1}, NY {2}", house.Address, house.City, house.ZipCode)).FirstOrDefault();

                if (address == null)
                {
                    continue;
                }

                house.Latitude = Convert.ToDecimal(address.Coordinates.Latitude);
                house.Longitude = Convert.ToDecimal(address.Coordinates.Longitude);
            }

            db.SaveChanges();

            return RedirectToAction("Index");
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