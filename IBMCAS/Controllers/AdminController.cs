using IBMCAS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace IBMCAS.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        // GET: Admin
        Models.IBMCASDBEntities1 _db = new Models.IBMCASDBEntities1();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AddNewPhysician()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddNewPhysician(Models.Physician physician)
        {
            if (ModelState.IsValid)
            {
                _db.Physicians.Add(physician);
                _db.SaveChanges();

                UserCred usr = new UserCred();
                usr.UserReferneceToID = physician.PhysicianID;
                usr.UserName = physician.PhysicianName + DateTime.Now.ToString("ddMMyyhhmmss");
                usr.UserPassword = physician.PhysicianPhone;
                usr.UserRole = "PHYSICIAN";
                
                _db.UserCreds.Add(usr);
                _db.SaveChanges();

                return RedirectToAction("Index");
            }
            return View();
        }
    }
}