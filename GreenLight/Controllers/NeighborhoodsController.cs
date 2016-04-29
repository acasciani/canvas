using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using GreenLight;

namespace GreenLight.Controllers
{
    [Authorize]
    public class NeighborhoodsController : Controller
    {
        private GreenLightEntities db = new GreenLightEntities();
        
        private int NeighborhoodID { get; set; }

        public NeighborhoodsController()
        {
            if (System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
            {
                NeighborhoodID = int.Parse(System.Web.HttpContext.Current.User.Identity.Name);
            }
        }

        // GET: Neighborhoods/Edit/5
        public ActionResult Edit()
        {
            Neighborhood neighborhood = db.Neighborhoods.Find(NeighborhoodID);
            if (neighborhood == null)
            {
                return HttpNotFound();
            }
            return View(neighborhood);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "AdminEmail,Latitude,Longitude,ZoomIndex,AdminPassPhrase,AdminUserName,Name,NeighborhoodID,URLName")] Neighborhood neighborhood)
        {
            if (ModelState.IsValid)
            {
                db.Entry(neighborhood).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Edit");
            }
            return View();
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
