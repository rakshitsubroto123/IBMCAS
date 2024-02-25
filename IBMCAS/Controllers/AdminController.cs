﻿using IBMCAS.Models;
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
        Models.IBMCASDBEntities2 _db = new Models.IBMCASDBEntities2();
        public ActionResult Index()
        {
            return RedirectToAction("Frontdesk");
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
            return View("~/Views/Patient/ListAll.cshtml","_LayoutAdmin", _db.Patients.ToList());
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
            return View("~/Views/Patient/Details.cshtml","_LayoutAdmin", _db.Patients.Where(q => q.PatientMRNumber == id).SingleOrDefault());
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
            return View("~/Views/Physician/ListAll.cshtml","_LayoutAdmin", _db.Physicians.ToList());
        }

        public ActionResult AddPhysician()
        {
            return View("~/Views/Physician/Add.cshtml","_LayoutAdmin");
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

        public ActionResult EditPhysician(int id)
        {
            return View("~/Views/Physician/Edit.cshtml", "_LayoutAdmin", _db.Physicians.Where(q => q.PhysicianID == id).SingleOrDefault());
        }

        [HttpPost]
        public ActionResult EditPhysician(Models.Physician phy)
        {
            if (ModelState.IsValid)
            {
                _db.Entry(phy).State = EntityState.Modified;
                _db.SaveChanges();
            }
            return View("PhysicianIndex", _db.Physicians.ToList());
        }

        public ActionResult DetailsPhysician(int id)
        {
            
            return View("~/Views/Physician/Details.cshtml","_LayoutAdmin", _db.Physicians.Find(id));
        }

        [HttpGet]
        public ActionResult AppointmentRequests()
        {
            var appointmentReq = from appoint in _db.AppointmentRequests
                                 where appoint.status == 0
                                 select appoint;
            return View(appointmentReq.ToList());
        }

        public ActionResult Schedule(string id)
        {
            AppointmentRequest appreq = _db.AppointmentRequests.Where(ar => ar.AppointmentRequestToken == id).FirstOrDefault();
            if (appreq == null)
            {
                return HttpNotFound();
            }
            return View(appreq);
        }

        [HttpPost]
        public ActionResult Schedule(Models.AppointmentRequest appreq)
        {
            if (ModelState.IsValid)
            {
                appreq.status = 1;
                _db.Entry(appreq).State = EntityState.Modified;
                Appointment appointment = new Appointment();
                appointment.AppointmentToken = DateTime.Now.Year.ToString() + DateTime.Now.TimeOfDay.ToString("hhmmss");
                appointment.PatientID = appreq.PatientID;
                appointment.PhysicianID = appreq.PhysicianID;
                appointment.ScheduledDate = appreq.DateScheduled;
                appointment.ScheduledTime = appreq.TimeScheduled;
                _db.Appointments.Add(appointment);
                _db.SaveChanges();
            }
            return RedirectToAction("AppointmentRequests");
        }

        public ActionResult RejectAppointmentRequest(string id)
        {
            AppointmentRequest appreq = _db.AppointmentRequests.Where(q => q.AppointmentRequestToken == id).SingleOrDefault();
            appreq.status = -1;
            _db.Entry(appreq).State = EntityState.Modified;
            _db.SaveChanges();
            return RedirectToAction("AppointmentRequests");
        }

        public ActionResult FrontDesk()
        {
            var todaysAppointment = from appoint in _db.Appointments
                                    where appoint.ScheduledDate == DateTime.Today.Date
                                    select appoint;

            return View(todaysAppointment.ToList());
        }

        //public ActionResult Isvisited(string id)
        //{
        //    Appointment app = _db.Appointments.Where(a => a.AppointmentToken == id).SingleOrDefault();
        //    app.isVisited = 1;
        //    _db.Entry(app).State = EntityState.Modified;
        //    _db.SaveChanges();
        //    return RedirectToAction("FrontDesk");
        //}

        public ActionResult chemistIndex()
        {
            return View(_db.chemists.ToList());
        }

        public ActionResult chemistCreate()
        {
            return View();
        }

        [HttpPost]
        public ActionResult chemistCreate(chemist chemist)
        {
            chemist newChemist = new chemist();
            newChemist.chemistName = chemist.chemistName;
            newChemist.chemistPhone = chemist.chemistPhone;
            newChemist.chemistEmail = chemist.chemistEmail;
            newChemist.chemistAddress = chemist.chemistAddress;
            newChemist.ChemistDateOfBirth = chemist.ChemistDateOfBirth;
            _db.chemists.Add(newChemist);
            _db.SaveChanges();

            UserCred usr = new UserCred();
            usr.UserName = chemist.chemistEmail;
            usr.UserPassword = chemist.chemistEmail;
            usr.UserRole = "CHEMIST";
            usr.UserReferneceToID =  _db.chemists.Where(q => q.chemistEmail == chemist.chemistEmail).SingleOrDefault().chemistId;
            //usr.UserReferneceToID = chemist.chemistId;

            _db.UserCreds.Add(usr);
            _db.SaveChanges();
            return View("chemistIndex", _db.chemists.ToList());
        }

        public ActionResult deleteChemist(int id)
        {
            _db.chemists.Remove(_db.chemists.Where(q => q.chemistId == id).SingleOrDefault());
            return View("chemistIndex", _db.chemists.ToList());
        }

        public ActionResult editChemist(int id)
        {
            return View(_db.chemists.Where(q => q.chemistId == id).SingleOrDefault());
        }

        [HttpPost]
        public ActionResult editChemist(chemist che)
        {
            if (ModelState.IsValid)
            {
                _db.Entry(che).State = EntityState.Modified;
                _db.SaveChanges();
            }
            return View("chemistIndex", _db.chemists.ToList());
        }

        public ActionResult DetailsChemist(int id)
        {
            return View(_db.chemists.Where(q => q.chemistId == id).SingleOrDefault());
        }

        public ActionResult IndexSupplier()
        {
            return View(_db.Suppliers.ToList());
        }

        public ActionResult CreateSupplier()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateSupplier([Bind(Include = "SupplierId, CompanyName, ContactNumber")] Supplier supplier)
        {
            Supplier newsup = new Supplier();
            newsup.CompanyName = supplier.CompanyName;
            newsup.ContactNumber = supplier.ContactNumber;
            _db.Suppliers.Add(newsup);
            _db.SaveChanges();

            UserCred newUsr = new UserCred();
            newUsr.UserName = supplier.CompanyName+ "@gmail.com";
            newUsr.UserRole = "SUPPLIER";
            newUsr.UserReferneceToID = _db.Suppliers.Where(q => q.CompanyName == supplier.CompanyName).SingleOrDefault().SupplierId;
            newUsr.UserPassword = supplier.CompanyName + "@gmail.com";
            _db.UserCreds.Add(newUsr);
            _db.SaveChanges();
            return View("IndexSupplier", _db.Suppliers.ToList());
        }

        public ActionResult EditSupplier(int id)
        {
            return View(_db.Suppliers.Where(q=>q.SupplierId==id).SingleOrDefault());
        }

        [HttpPost]
        public ActionResult EditSupplier(Supplier sup)
        {
            if(ModelState.IsValid)
            {
                _db.Entry(sup).State = EntityState.Modified;
                _db.SaveChanges();
            }
            return View("IndexSupplier", _db.Suppliers.ToList());
        }

        public ActionResult DetailsSupplier(int id)
        {
            return View(_db.Suppliers.Where(q=>q.SupplierId==id).SingleOrDefault());
        }
    }
}