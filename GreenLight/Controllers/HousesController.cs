using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using GreenLight;
using System.Web.Security;

namespace GreenLight.Controllers
{
    [Authorize]
    public class HousesController : Controller
    {
        private GreenLightEntities db = new GreenLightEntities();

        private int NeighborhoodID { get; set; }

        public HousesController()
        {
            if (System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
            {
                NeighborhoodID = int.Parse(System.Web.HttpContext.Current.User.Identity.Name);
            }
        }

        // GET: Houses
        public ActionResult Index()
        {
            return View(db.Houses.Where(i => i.NeighborhoodID == NeighborhoodID).OrderBy(i => i.Address).ToList());
        }

        // GET: Houses/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            House house = db.Houses.Where(i => i.NeighborhoodID == NeighborhoodID && i.HouseID == id.Value).FirstOrDefault();
            if (house == null)
            {
                return HttpNotFound();
            }
            return View(house);
        }

        // GET: Houses/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Houses/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "HouseID,Address")] House house)
        {
            if (ModelState.IsValid)
            {
                house.NeighborhoodID = NeighborhoodID;
                db.Houses.Add(house);
                db.SaveChanges();
                return RedirectToAction("Create");
            }

            return View(house);
        }

        // GET: Houses/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            House house = db.Houses.Where(i => i.NeighborhoodID == NeighborhoodID && i.HouseID == id.Value).FirstOrDefault();
            if (house == null)
            {
                return HttpNotFound();
            }
            return View(house);
        }

        // POST: Houses/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "HouseID,Address,Latitude,Longitude")] House house)
        {
            if (ModelState.IsValid)
            {
                db.Entry(house).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(house);
        }

        // GET: Houses/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            House house = db.Houses.Where(i => i.HouseID == id && i.NeighborhoodID == NeighborhoodID).FirstOrDefault();
            if (house == null)
            {
                return HttpNotFound();
            }
            return View(house);
        }

        // POST: Houses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            House house = db.Houses.Where(i => i.HouseID == id && i.NeighborhoodID == NeighborhoodID).FirstOrDefault();
            db.Houses.Remove(house);
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
