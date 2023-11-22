using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Zero_Hunger.EF;

namespace Zero_Hunger.Controllers
{
    public class EmployeeController : Controller
    {
        // GET: Employee
        [Authorize]
        public ActionResult Panel()
        {
            var db = new zeroHEntities1();
            var data = db.Collections.ToList();
            return View(data);
        }
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Register(Employee emp)
        {
            var db = new zeroHEntities1();
            db.Employees.Add(emp);
            db.SaveChanges();
            TempData["error"] = "Successfully Created.";
            return RedirectToAction("CollectionRequest", "Donate");
        }

        public ActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Panel", "Employee");
            }
            return View();
        }

        [HttpPost]
        public ActionResult Login(Employee emp)
        {
            var db = new zeroHEntities1();
            var em = (from ad in db.Employees
                      where ad.Username == emp.Username && ad.Password == emp.Password
                      select ad).FirstOrDefault();
            if (em != null)
            {
                string Uname = em.Username.ToString();
                FormsAuthentication.SetAuthCookie(Uname, false);
                return RedirectToAction("Panel", "Employee");
            }
            TempData["error"] = "Worng username or password";
            return View();
        }
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }
        
        [Authorize]
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(Employee emp)
        {
            var db = new zeroHEntities1();
            db.Employees.Add(emp);
            db.SaveChanges();
            TempData["error"] = "Successfully Created.";
            return RedirectToAction("AdminPanel", "Admin");
        }
        [HttpGet]
        [Authorize]
        public ActionResult Edit(int id)
        {
            var db = new zeroHEntities1();
            var emp = (from pd in db.Employees
                       where pd.Id == id
                       select pd).FirstOrDefault();
            return View(emp);
        }

        [HttpPost]
        [Authorize]
        public ActionResult EditEmployee(Employee emp)
        {
            var db = new zeroHEntities1();
            var e = (from pd in db.Employees
                     where pd.Id == emp.Id
                     select pd).FirstOrDefault();
            e.Name = emp.Name;
            e.DOB = emp.DOB;
            e.Location = emp.Location;
            e.Phone = emp.Phone;
            e.Email = emp.Email;
            e.Username = emp.Username;
            e.Password = emp.Password;
            db.SaveChanges();
            TempData["error"] = "Successfully Edited.";
            return RedirectToAction("Index");
        }
        [Authorize]
        public ActionResult DeleteEmployee(int id)
        {
            var db = new zeroHEntities1();
            var a = db.Employees.Where(i => i.Id == id).SingleOrDefault();
            db.Employees.Remove(a);
            db.SaveChanges();
            TempData["error"] = "Successfully deleted.";
            return RedirectToAction("Index");

        }

        [Authorize]
        public ActionResult Details(int id)
        {
            var db = new zeroHEntities1();
            var e = (from pd in db.Employees
                     where pd.Id == id
                     select pd).FirstOrDefault();
            return View(e);
        }

        [Authorize]
        public ActionResult Complete(int id)
        {
            var db = new zeroHEntities1();
            var a = db.Collections.Where(i => i.Id == id).SingleOrDefault();
            a.Status = "Done";
            db.SaveChanges();
            TempData["error"] = "Successfully Completed.";
            return RedirectToAction("Panel");
        }

        [HttpGet]
        public ActionResult ChangePassword()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ChangePassword(FormCollection form)
        {
            var user = User.Identity.Name;
            string Opass = form["Old_Password"];
            string Npass = form["New_Password"];
            string Cpass = form["Con_Password"];
            var db = new zeroHEntities1();

            if (Opass != null && Npass != null && Cpass != null)
            {
                var us = (from ad in db.Employees
                          where ad.Username == user && ad.Password == Opass
                          select ad).FirstOrDefault();
                if (us != null)
                {
                    if (Npass == Cpass)
                    {
                        if (User.Identity.IsAuthenticated)
                        {

                            var n_P = (from pd in db.Employees
                                       where pd.Username == user
                                       select pd).FirstOrDefault();
                            n_P.Password = Npass;
                            db.SaveChanges();
                            TempData["error"] = "Successfully Changed password.";
                            return RedirectToAction("Panel", "Employee");

                        }
                        else
                            return RedirectToAction("Login", "Employee");
                    }
                    else
                    {
                        TempData["error"] = "New password and confirm password doesn't match!";
                        return View();
                    }
                }
                else
                {
                    TempData["error"] = "Incorrect password!";
                    return View();
                }

            }
            else
            {
                TempData["error"] = "Please fill up information properly!";
            }
            return View();



        }


    }
}