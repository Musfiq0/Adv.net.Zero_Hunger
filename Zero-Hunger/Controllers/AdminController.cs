using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Zero_Hunger.EF;


namespace Zero_Hunger.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        [Authorize]
        public ActionResult AdminPanel()
        {
            return View();
        }
        [HttpGet]

        public ActionResult Login()

        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("AdminPanel", "Admin");
            }
            return View();
        }
        [HttpPost]
        public ActionResult Login(Admin admin)
        {

            var db = new zeroHEntities1();
            var us = (from ad in db.Admins
                      where ad.username == admin.username && ad.Password == admin.Password
                      select ad).FirstOrDefault();
            if (us != null)
            {
                string Uname = us.username.ToString();
                FormsAuthentication.SetAuthCookie(Uname, false);
                return RedirectToAction("AdminPanel", "Admin");
            }
            TempData["error"] = "Wrong username or password";
            return View();
        }
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }
        [Authorize]
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
                var am = (from ad in db.Admins
                          where ad.username == user && ad.Password == Opass
                          select ad).FirstOrDefault();
                if (am != null)
                {
                    if (Npass == Cpass)
                    {
                        if (User.Identity.IsAuthenticated)
                        {

                            var n_P = (from pd in db.Admins
                                       where pd.username == user
                                       select pd).FirstOrDefault();
                            n_P.Password = Npass;
                            db.SaveChanges();
                            TempData["error"] = "Successfully Changed password.";
                            return RedirectToAction("AdminPanel", "Admin");

                        }
                        else
                            return RedirectToAction("Login", "Admin");
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

        [Authorize]
        public ActionResult EmployeeManage()
        {
            var db = new zeroHEntities1();
            var emp = db.Employees.ToList();
            return View(emp);
        }

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
            return RedirectToAction("EmployeeManage");

        }
        [Authorize]
        public ActionResult EmployeeDetails(int id)
        {
            var db = new zeroHEntities1();
            var b = (from pd in db.Employees
                     where pd.Id == id
                     select pd).FirstOrDefault();
            return View(b);
        }
    }
}