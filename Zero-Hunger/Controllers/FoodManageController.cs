using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Zero_Hunger.EF;
using Zero_Hunger.Models;

namespace Zero_Hunger.Controllers
{
    public class FoodManageController : Controller
    {
        // GET: FoodManage
        [Authorize]
        public ActionResult Index()
        {
            var db = new zeroHEntities1();
            var donat = db.Donations.ToList();
            return View(donat);
        }
        [Authorize]
        public ActionResult OnlyCollect()
        {
            var db = new zeroHEntities1();
            var don = (from pd in db.Donations
                       where pd.Status == "Collectable"
                       select pd).ToList();
            return View(don);
        }
        [Authorize]

        public ActionResult Processed()
        {
            var db = new zeroHEntities1();
            var don = (from pd in db.Donations
                       where pd.Status == "Processed"
                       select pd).ToList();
            return View(don);
        }
        [Authorize]
        public ActionResult OnlyDistribute()
        {
            var db = new zeroHEntities1();
            var don = (from pd in db.Donations
                       where pd.Status == "Deliverable"
                       select pd).ToList();
            return View(don);
        }
        [Authorize]

        [HttpGet]
        public ActionResult Collect(int id)
        {
            var db = new zeroHEntities1();
            var don = (from pd in db.Donations
                       where pd.Id == id
                       select pd).ToList();
            var emp = db.Employees.ToList();
            var colList = db.Collections.ToList();

            info_class info_Class = new info_class();
            info_Class.emp = emp;
            info_Class.don = don;
            info_Class.col = colList;

            return View(info_Class);
        }
        [HttpPost]
        public ActionResult Collect(int DonId, string DonLoc, string DonAdd, String ExpIn, int empId)
        {
            var db = new zeroHEntities1();
            var emp = (from ad in db.Employees
                       where ad.Id == empId
                       select ad).FirstOrDefault();
            var food = (from ad in db.Donations
                        where ad.Id == DonId
                        select ad).FirstOrDefault();
            Collection colec = new Collection();
            Donation dona = new Donation();

            if (emp != null && food != null)
            {
                colec.EmpId = empId;
                colec.DonationId = DonId;
                colec.Status = "In Process";
                db.Collections.Add(colec);
                db.SaveChanges();

                var status_Change = (from ad in db.Donations
                                     where ad.Id == DonId
                                     select ad).FirstOrDefault();
                status_Change.Status = "Deliverable";
                db.SaveChanges();
                TempData["error"] = "Successfully Assigned!.";
                return RedirectToAction("Index");

            }
            else
            {
                TempData["error"] = "Something wrong!.";
                return View();
            }
        }

        [Authorize]
        [HttpGet]
        public ActionResult Distribute(int id)
        {
            var db = new zeroHEntities1();
            var don = (from pd in db.Donations
                       where pd.Id == id
                       select pd).ToList();
            var emp = db.Employees.ToList();

            var colList = db.Collections.ToList();

            info_class info_Class = new info_class();
            info_Class.emp = emp;
            info_Class.don = don;
            info_Class.col = colList;


            return View(info_Class);
        }
        [HttpPost]
        public ActionResult Distribute(int DonId, string DonLoc, string DonAdd, String ExpIn, int empId)
        {
            var db = new zeroHEntities1();
            var emp = (from ad in db.Employees
                       where ad.Id == empId
                       select ad).FirstOrDefault();
            var food = (from ad in db.Donations
                        where ad.Id == DonId
                        select ad).FirstOrDefault();
            Collection colec = new Collection();
            Donation dona = new Donation();

            if (emp != null && food != null)
            {
                colec.EmpId = empId;
                colec.DonationId = DonId;
                colec.Status = "In Process";
                db.Collections.Add(colec);
                db.SaveChanges();

                var status_Change = (from ad in db.Donations
                                     where ad.Id == DonId
                                     select ad).FirstOrDefault();
                status_Change.Status = "Processed";
                db.SaveChanges();
                TempData["error"] = "Successfully Assigned!.";
                return RedirectToAction("Index");

            }
            else
            {
                TempData["error"] = "Something wrong!.";
                return View();
            }
        }
        [Authorize]
        public ActionResult Track()
        {
            var db = new zeroHEntities1();
            return View(db.Collections.ToList());
        }
    
    }
}