using System;
using System.Web.Mvc;

namespace IBMCAS.Controllers
{
    public class HomeController : Controller
    {
        Models.IBMCASDBEntities2 _db = new Models.IBMCASDBEntities2();
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(Models.PatientRegistrationQueue patientRegistrationQueue)
        {
            if (ModelState.IsValid)
            {
                patientRegistrationQueue.RegistrationTokenNo = "PRQ" + patientRegistrationQueue.PatientDOB.Date.Year.ToString() + DateTime.Now.ToString("ddMMyyhhmmss");
                patientRegistrationQueue.DateCreated = DateTime.Now.Date;
                _db.PatientRegistrationQueues.Add(patientRegistrationQueue);
                _db.SaveChanges();
                ViewBag.PatientRegistrationFormData = patientRegistrationQueue.RegistrationTokenNo;
                return View("RegisterSuccessfulView");
            }
            return View();
        }
    }
}