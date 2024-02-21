using IBMCAS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.UI.WebControls;

namespace IBMCAS.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(UserModel user)
        {
            if (ModelState.IsValid)
            {
                Models.IBMCASDBEntities2 _db = new Models.IBMCASDBEntities2();
                UserCred usr = _db.UserCreds.SingleOrDefault(dbusr => dbusr.UserName.ToLower()
                == user.UserName.ToLower() && dbusr.UserPassword == user.UserPassword);

                if (usr != null)
                {
                    FormsAuthentication.SetAuthCookie(usr.UserName, false);
                    CurrentUserModel cusr = new CurrentUserModel();
                    cusr.UserName = usr.UserName;
                    cusr.ReferenceToId = usr.UserReferneceToID;
                    cusr.UserID = usr.UserID;
                    cusr.Role = usr.UserRole;

                    Session["CurrentUser"] = cusr;

                    return RedirectToAction("Index", usr.UserRole);
                }
            }
            ModelState.AddModelError("", "invalid Entry of username and password");
            return View("Index");
            //return new RedirectResult(Url.Action("Index") + "#login");
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(Models.PatientRegistrationForm patientRegistrationFormData)
        {
            if (ModelState.IsValid)
            {
                Models.PatientRegistrationQueue patientRegistrationQueue = new Models.PatientRegistrationQueue();
                patientRegistrationQueue.RegistrationTokenNo = "PRQ" + patientRegistrationFormData.DateOfBirth.Date.Year.ToString() + DateTime.Now.ToString("ddMMyyhhmmss");
                patientRegistrationQueue.DateCreated = DateTime.Now.Date;
                patientRegistrationQueue.PatientDOB = DateTime.Now.Date;
                patientRegistrationQueue.PatientFirstName = patientRegistrationFormData.FirstName;
                patientRegistrationQueue.PatientLastName = patientRegistrationFormData.LastName;
                patientRegistrationQueue.PatientAddress = patientRegistrationFormData.Address;
                patientRegistrationQueue.PatientPhone = patientRegistrationFormData.PhoneNumber;
                patientRegistrationQueue.PatientEmail = patientRegistrationFormData.Email;
                patientRegistrationQueue.PatientGender = patientRegistrationFormData.Gender.ToString();
                patientRegistrationQueue.PatientMedicalHistory = patientRegistrationFormData.MedicalHistory;
                patientRegistrationQueue.PatientAadhaarNumber = patientRegistrationFormData.AadhaarNumber;

                Models.IBMCASDBEntities2 _db = new Models.IBMCASDBEntities2();
                _db.PatientRegistrationQueues.Add(patientRegistrationQueue);
                _db.SaveChanges();
                ViewBag.PatientRegistrationFormData = patientRegistrationQueue.RegistrationTokenNo;
                return View("RegisterSuccessfulView");

            }
            return View();
        }
    }
}