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
    public class VisitsController : Controller
    {
        private GreenLightEntities db = new GreenLightEntities();

        private int NeighborhoodID { get; set; }

        private List<int> HouseIDs { get; set; }

        public VisitsController()
        {
            if (System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
            {
                NeighborhoodID = int.Parse(System.Web.HttpContext.Current.User.Identity.Name);
                HouseIDs = db.Houses.Where(i => i.NeighborhoodID == NeighborhoodID).Select(i => i.HouseID).Distinct().ToList();
            }
        }

        // GET: Visits
        public ActionResult Index()
        {
            var visits = db.Visits.Include(v => v.House).Include(v => v.Respons).Where(i => HouseIDs.Contains(i.HouseID));
            return View(visits.ToList());
        }

        // GET: Visits/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Visit visit = db.Visits.Where(i => i.VisitID == id.Value && HouseIDs.Contains(i.HouseID)).FirstOrDefault();
            if (visit == null)
            {
                return HttpNotFound();
            }
            return View(visit);
        }

        // GET: Visits/Create
        public ActionResult Create(int? id)
        {
            House house = db.Houses.FirstOrDefault(i => i.HouseID == id && i.NeighborhoodID == NeighborhoodID);

            ViewBag.House = house;
            ViewBag.HouseID = new SelectList(db.Houses.Where(i => i.NeighborhoodID == NeighborhoodID).OrderBy(i => i.Address), "HouseID", "Address");
            ViewBag.ResponseID = new SelectList(db.Responses.OrderBy(i => i.SortOrder), "ResponseID", "Name");
            return View();
        }

        // POST: Visits/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "VisitID,HouseID,ResponseID,Notes")] Visit visit)
        {
            if (ModelState.IsValid)
            {
                visit.CreateDate = DateTime.Now.ToUniversalTime();
                db.Visits.Add(visit);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.HouseID = new SelectList(db.Houses, "HouseID", "Address", visit.HouseID);
            ViewBag.ResponseID = new SelectList(db.Responses, "ResponseID", "Name", visit.ResponseID);
            return View(visit);
        }

        // GET: Visits/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Visit visit = db.Visits.Where(i => i.VisitID == id.Value && HouseIDs.Contains(i.HouseID)).FirstOrDefault();
            if (visit == null)
            {
                return HttpNotFound();
            }

            ViewBag.House = visit.House;
            ViewBag.HouseID = new SelectList(db.Houses, "HouseID", "Address", visit.HouseID);
            ViewBag.ResponseID = new SelectList(db.Responses, "ResponseID", "Name", visit.ResponseID);
            return View(visit);
        }

        // POST: Visits/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "VisitID,HouseID,ResponseID,Notes")] Visit visit)
        {
            if (ModelState.IsValid)
            {
                db.Entry(visit).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.HouseID = new SelectList(db.Houses, "HouseID", "Address", visit.HouseID);
            ViewBag.ResponseID = new SelectList(db.Responses, "ResponseID", "Name", visit.ResponseID);
            return View(visit);
        }

        // GET: Visits/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Visit visit = db.Visits.Where(i => i.VisitID == id.Value && HouseIDs.Contains(i.HouseID)).FirstOrDefault();
            if (visit == null)
            {
                return HttpNotFound();
            }
            return View(visit);
        }

        // POST: Visits/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Visit visit = db.Visits.Where(i => i.VisitID == id && HouseIDs.Contains(i.HouseID)).FirstOrDefault();
            db.Visits.Remove(visit);
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