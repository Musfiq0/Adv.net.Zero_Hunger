using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Zero_Hunger.EF;

namespace Zero_Hunger.Controllers
{
    public class zeroHController : Controller
    {
        // GET: zeroH
        public ActionResult Home()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Register(Donar don)
        {
            var db = new zeroHEntities1();
            db.Donars.Add(don);
            db.SaveChanges();
            TempData["error"] = "Successfully Created.";
            return RedirectToAction("Login");
        }
        [HttpGet]
        public ActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "zeroH");
            }
            return View();
        }

        [HttpPost]
        public ActionResult Login(Donar don)
        {
            var db = new zeroHEntities1();
            var us = (from ad in db.Donars
                      where ad.username == don.username && ad.Password == don.Password
                      select ad).FirstOrDefault();
            if (us != null)
            {
                string Uname = us.username.ToString();
                FormsAuthentication.SetAuthCookie(Uname, false);
                return RedirectToAction("CollectionRequest", "Donate");
            }
            TempData["error"] = "Worng username or password";
            return View();
        }
    }
}