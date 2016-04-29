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
    public class OptionsController : Controller
    {
        private GreenLightEntities db = new GreenLightEntities();

        private int NeighborhoodID { get; set; }

        public OptionsController()
        {
            if (System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
            {
                NeighborhoodID = int.Parse(System.Web.HttpContext.Current.User.Identity.Name);
            }
        }

        // GET: Options
        public ActionResult Index()
        {
            var options = db.Options.Where(i=>i.NeighborhoodID == NeighborhoodID).Include(o => o.OptionType);
            return View(options.ToList());
        }

        // GET: Options/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Option option = db.Options.Where(i => i.OptionID == id && i.NeighborhoodID == NeighborhoodID).FirstOrDefault();
            if (option == null)
            {
                return HttpNotFound();
            }
            return View(option);
        }

        // GET: Options/Create
        public ActionResult Create()
        {
            ViewBag.NeighborhoodID = new SelectList(db.Neighborhoods.Where(i => i.NeighborhoodID == NeighborhoodID), "NeighborhoodID", "Name");
            ViewBag.OptionTypeID = new SelectList(db.OptionTypes, "OptionTypeID", "OptionTypeName");
            return View();
        }

        // POST: Options/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "OptionID,Value,NeighborhoodID,OptionTypeID")] Option option)
        {
            if (ModelState.IsValid)
            {
                db.Options.Add(option);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.NeighborhoodID = new SelectList(db.Neighborhoods.Where(i => i.NeighborhoodID == NeighborhoodID), "NeighborhoodID", "Name", option.NeighborhoodID);
            ViewBag.OptionTypeID = new SelectList(db.OptionTypes, "OptionTypeID", "OptionTypeName", option.OptionTypeID);
            return View(option);
        }

        // GET: Options/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Option option = db.Options.Where(i => i.OptionID == id && i.NeighborhoodID == NeighborhoodID).FirstOrDefault();
            if (option == null)
            {
                return HttpNotFound();
            }
            ViewBag.NeighborhoodID = new SelectList(db.Neighborhoods.Where(i => i.NeighborhoodID == NeighborhoodID), "NeighborhoodID", "Name", option.NeighborhoodID);
            ViewBag.OptionTypeID = new SelectList(db.OptionTypes, "OptionTypeID", "OptionTypeName", option.OptionTypeID);
            return View(option);
        }

        // POST: Options/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "OptionID,Value,NeighborhoodID,OptionTypeID")] Option option)
        {
            if (ModelState.IsValid)
            {
                db.Entry(option).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.NeighborhoodID = new SelectList(db.Neighborhoods.Where(i => i.NeighborhoodID == NeighborhoodID), "NeighborhoodID", "Name", option.NeighborhoodID);
            ViewBag.OptionTypeID = new SelectList(db.OptionTypes, "OptionTypeID", "OptionTypeName", option.OptionTypeID);
            return View(option);
        }

        // GET: Options/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Option option = db.Options.Where(i => i.OptionID == id && i.NeighborhoodID == NeighborhoodID).FirstOrDefault();
            if (option == null)
            {
                return HttpNotFound();
            }
            return View(option);
        }

        // POST: Options/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Option option = db.Options.Where(i => i.OptionID == id && i.NeighborhoodID == NeighborhoodID).FirstOrDefault();
            db.Options.Remove(option);
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