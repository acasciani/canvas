using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using System.Web.Security;

namespace GreenLight.Controllers
{
    public class AccountController : Controller
    {
        private GreenLightEntities db = new GreenLightEntities();

        // GET: Account
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login(Neighborhood neighborhood, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(neighborhood);
            }

            // Require the user to have a confirmed email before they can log on.
            var user = db.Neighborhoods.FirstOrDefault(i => neighborhood.AdminUserName.ToUpper() == i.AdminUserName.ToUpper() && neighborhood.AdminPassPhrase == i.AdminPassPhrase);

            if (user != null)
            {
                FormsAuthentication.SetAuthCookie(user.NeighborhoodID.ToString(), true);
                return RedirectToAction("Index", "Houses");
            }
            else
            {
                ModelState.AddModelError("", "Invalid login attempt.");
                return View(neighborhood);
            }
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