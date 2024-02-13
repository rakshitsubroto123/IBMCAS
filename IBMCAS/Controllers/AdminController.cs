using IBMCAS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace IBMCAS.Controllers
{
    [Authorize(Roles = "ADMIN")]
    public class AdminController : Controller
    {
        // GET: Admin
        Models.IBMCASDBEntities1 _db = new Models.IBMCASDBEntities1();
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult RegistrationRequests() 
        {
            return View(_db.PatientRegistrationQueues.ToList());
        }

        [HttpGet]
        public ActionResult ShowOneRegistrationRequest(string id)
        {
            return View(_db.PatientRegistrationQueues.Where(q => q.RegistrationTokenID == id).SingleOrDefault());
        }

        [HttpGet]
        public ActionResult EditRegistrationRequest(string id)
        {
            return View(_db.PatientRegistrationQueues.Where(q => q.RegistrationTokenID == id).SingleOrDefault());
        }

        [HttpGet]
        public ActionResult ApproveRegistrationRequest(string id) 
        {
            PatientRegistrationQueue prq = _db.PatientRegistrationQueues.FirstOrDefault(q => q.RegistrationTokenID == id);
            Patient patient = new Patient();
            patient.PatientFirstName = prq.PatientFirstName;
            patient.PatientLastName = prq.PatientLastName;
            patient.PatientGender = prq.PatientGender;
            patient.PatientDOB = prq.PatientDOB;
            patient.PatientPhone = prq.PatientPhone;
            patient.PatientEmail = prq.PatientEmail;
            patient.PatientMedicalHistory = prq.PatientMedicalHistory;
            patient.PatientAddress = prq.PatientAddress;
            patient.PatientAadhaarNumber = prq.PatientAadhaarNumber;
            patient.PatientMRNumber = "MR_" + DateTime.Now.ToString("ddMMyyyyhhmmss");
            _db.Patients.Add(patient);
            _db.SaveChanges();

            int patientID = _db.Patients.Where(p => p.PatientMRNumber == patient.PatientMRNumber).FirstOrDefault().PatientID;
            UserCred userCred = new UserCred();
            userCred.UserName = patient.PatientEmail;
            userCred.UserPassword = patient.PatientEmail;
            userCred.UserRole = Role.ADMIN.ToString();
            userCred.UserReferneceToID = patientID;
            _db.UserCreds.Add(userCred);
            _db.PatientRegistrationQueues.Remove(_db.PatientRegistrationQueues.Where(p => p.RegistrationTokenID == id).Single());
            _db.SaveChanges();

            return RedirectToAction("RegistrationRequests");
        }

        [HttpGet]
        public ActionResult RejectRegistrationRequest(string id)
        {
            _db.PatientRegistrationQueues.Remove(_db.PatientRegistrationQueues.Where(p => p.RegistrationTokenID == id).Single());
            _db.SaveChanges();
            return RedirectToAction("RegistrationRequests");
        }
    }
}