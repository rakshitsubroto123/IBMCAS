using IBMCAS.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
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
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PatientRegistrationQueue patientRegistrationQueue = _db.PatientRegistrationQueues.Where(q => q.RegistrationTokenNo == id).SingleOrDefault();
            if (patientRegistrationQueue == null)
            {
                return HttpNotFound();
            }
            return View(patientRegistrationQueue);
            //return View(_db.PatientRegistrationQueues.Where(q => q.RegistrationTokenID == id).SingleOrDefault());
        }

        [HttpGet]
        public ActionResult EditRegistrationRequest(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PatientRegistrationQueue patientRegistrationQueue = _db.PatientRegistrationQueues.Where(q => q.RegistrationTokenNo == id).SingleOrDefault();
            if (patientRegistrationQueue == null)
            {
                return HttpNotFound();
            }
            return View(patientRegistrationQueue);

        }

        [HttpPost]
        public ActionResult EditRegistrationRequest([Bind(    Include = "PRQID, RegistrationTokenNo,DateCreated,PatientDOB,PatientFirstName,PatientLastName,PatientAddress,PatientPhone,PatientEmail,PatientGender,PatientMedicalHistory,PatientAadhaarNumber")] PatientRegistrationQueue patientRegistrationQueue)
        {
            if (ModelState.IsValid)
            {
                _db.Entry(patientRegistrationQueue).State = EntityState.Modified;
                _db.SaveChanges();
            }
            return RedirectToAction("ShowOneRegistrationRequest", new {id = patientRegistrationQueue.RegistrationTokenNo});
        }

        [HttpGet]
        public ActionResult ApproveRegistrationRequest(string id) 
        {
            PatientRegistrationQueue prq = _db.PatientRegistrationQueues.FirstOrDefault(q => q.RegistrationTokenNo == id);
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
            patient.PatientMRNumber = "MR_" + DateTime.Now.Year + DateTime.Now.TimeOfDay.ToString("hhmmss");
            _db.Patients.Add(patient);
            _db.SaveChanges();

            int patientID = _db.Patients.Where(p => p.PatientMRNumber == patient.PatientMRNumber).FirstOrDefault().PatientID;
            UserCred userCred = new UserCred();
            userCred.UserName = patient.PatientEmail;
            userCred.UserPassword = patient.PatientEmail;
            userCred.UserRole = Role.PATIENT.ToString();
            userCred.UserReferneceToID = patientID;
            _db.UserCreds.Add(userCred);
            _db.PatientRegistrationQueues.Remove(_db.PatientRegistrationQueues.Where(p => p.RegistrationTokenNo == id).Single());
            _db.SaveChanges();

            return RedirectToAction("RegistrationRequests");
        }

        [HttpGet]
        public ActionResult RejectRegistrationRequest(string id)
        {
            _db.PatientRegistrationQueues.Remove(_db.PatientRegistrationQueues.Where(p => p.RegistrationTokenNo == id).Single());
            _db.SaveChanges();
            return RedirectToAction("RegistrationRequests");
        }


        public ActionResult PatientIndex()
        {
            return View("PatientIndex", _db.Patients.ToList());
        }

        public ActionResult AddPatient()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddPatient(Models.Patient patient)
        {
            patient.PatientMRNumber = "MR_" + DateTime.Now.Year + DateTime.Now.TimeOfDay.ToString("hhmmss");
            _db.Patients.Add(patient);
            _db.SaveChanges();

            UserCred userCred = new UserCred();
            userCred.UserName = patient.PatientEmail;
            userCred.UserPassword = patient.PatientEmail;
            userCred.UserRole = Role.PATIENT.ToString();
            userCred.UserReferneceToID = patient.PatientID;
            _db.UserCreds.Add(userCred);
            _db.SaveChanges();
            return RedirectToAction("PatientIndex");
        }

        public ActionResult ShowPatient(string id)
        {
            return View(_db.Patients.Where(q => q.PatientMRNumber == id).SingleOrDefault());
        }

        public ActionResult UpdatePatient(string id)
        {
            return View(_db.Patients.Where(q => q.PatientMRNumber == id).SingleOrDefault());
        }

        [HttpPost]
        public ActionResult UpdatePatient(Models.Patient patient)
        {
            if (ModelState.IsValid)
            {
                _db.Entry(patient).State = EntityState.Modified;
                _db.SaveChanges();
            }
            return RedirectToAction("ShowPatient", new { id = patient.PatientMRNumber });
        }

        public ActionResult PhysicianIndex()
        {
            return View("PhysicianIndex", _db.Physicians.ToList());
        }

        public ActionResult AddPhysician()
        {
            return View();
        }

        // POST: Physicians/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddPhysician([Bind(Include = "PhysicianName,PhysicianAddress,PhysicianPhone,PhysicianEmail,PhysicianSpecialization,PhysicianSummary")] Physician physician)
        {
            if (ModelState.IsValid)
            {
                if(_db.Physicians.Where(p => p.PhysicianEmail == physician.PhysicianEmail).SingleOrDefault() != default)
                {
                    ModelState.AddModelError("PhysicianEmail", "Email Already Exits");
                    return View("AddPhysician");
                }
                _db.Physicians.Add(physician);

                _db.SaveChanges();

                int physicianID = _db.Physicians.Where(p => p.PhysicianEmail == physician.PhysicianEmail).FirstOrDefault().PhysicianID;
                UserCred userCred = new UserCred();
                userCred.UserName = physician.PhysicianEmail;
                userCred.UserPassword = physician.PhysicianEmail;
                userCred.UserRole = Role.PHYSICIAN.ToString();
                userCred.UserReferneceToID = physicianID;
                _db.UserCreds.Add(userCred);
                _db.SaveChanges();
            }

            return RedirectToAction("PhysicianIndex");
        }

        [HttpGet]
        public ActionResult ApointmentRequests()
        {
            var appointmentReq = from appoint in _db.Appointments
                                 where appoint.ScheduledDate == null
                                 select appoint;
            return View(appointmentReq.ToList());
        }

        public ActionResult Schedule(string id)
        {
            return View(_db.Appointments.Where(q => q.AppointmentToken == id).SingleOrDefault());
        }

        [HttpPost]
        public ActionResult Schedule(Models.Appointment appointment)
        {
            if (ModelState.IsValid)
            {
                _db.Entry(appointment).State = EntityState.Modified;
                _db.SaveChanges();
            }
            return RedirectToAction("ApointmentRequests");
        }

        public ActionResult AppointmentRemove(string id)
        {
            _db.Appointments.Remove(_db.Appointments.Where(q => q.AppointmentToken == id).Single());
            _db.SaveChanges();
            return RedirectToAction("ApointmentRequests");
        }

        public ActionResult FrontDesk()
        {
            var todaysAppointment = from appoint in _db.Appointments
                                    where appoint.ScheduledDate == DateTime.Today.Date
                                    select appoint;

            return View(todaysAppointment.ToList());
        }

        public ActionResult Isvisited(string id)
        {
            Appointment app = _db.Appointments.Where(a => a.AppointmentToken == id).SingleOrDefault();
            app.isVisited = 1;
            _db.Entry(app).State = EntityState.Modified;
            _db.SaveChanges();
            return RedirectToAction("FrontDesk");
        }
    }
}