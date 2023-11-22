using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Zero_Hunger.EF;

namespace Zero_Hunger.Controllers
{
    public class DonateController : Controller
    {
        // GET: Donate
        [Authorize]
        [HttpGet]
        public ActionResult CollectionRequest()
        {

            return View();
        }

        [HttpPost]
        public ActionResult CollectionRequest(Donation donation)
        {
            var db = new zeroHEntities1();
            donation.Status = "Collectable";
            db.Donations.Add(donation);
            db.SaveChanges();
            TempData["error"] = "Successfully Donated!";
            return RedirectToAction("Index", "Donate");
        }
        public ActionResult Index()
        {
            var db = new zeroHEntities1();
            var donat = db.Donations.ToList();
            return View(donat);
        }
    }
}