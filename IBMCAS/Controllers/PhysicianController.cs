using IBMCAS.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IBMCAS.Controllers
{
    [Authorize(Roles = "PHYSICIAN")]

    public class PhysicianController : Controller
    {
        Models.IBMCASDBEntities2 _db = new Models.IBMCASDBEntities2();

        // GET: Physician
        public ActionResult Index()
        {
            return RedirectToAction("Dashboard");
        }

        public ActionResult Dashboard()
        {
            return RedirectToAction("Apointments");
        }

        public ActionResult Apointments()
        {
            IBMCAS.Models.CurrentUserModel usr = Session["CurrentUser"] as IBMCAS.Models.CurrentUserModel;
            return View(_db.Appointments.Where(q => q.PhysicianID == usr.ReferenceToId && q.ScheduledDate == DateTime.Today.Date).ToList());
        }

        //public ActionResult Prescription(string id)
        //{
        //    return View();
        //}

        //[HttpPost]
        //public ActionResult Prescription([Bind(Include = "PrescriptionAdviceId, DrugID, Prescription1")] Prescription prescription)
        //{
        //    IBMCAS.Models.CurrentUserModel usr = Session["CurrentUser"] as IBMCAS.Models.CurrentUserModel;
        //        Prescription newPres = new Prescription();
        //        newPres.PrescriptionAdviceId = _db.Appointments.Where(q => q.PhysicianID == usr.ReferenceToId).FirstOrDefault().AppointmentID;
        //        newPres.DrugID = prescription.DrugID;
        //        newPres.Prescription1 = prescription.Prescription1;
            
        //        //_db.Appointments.Where(q => q.PhysicianID 
        //        //                       == usr.ReferenceToId && 
        //        //                       _db.Prescriptions.Where(p => p.Prescribed 
        //        //                       == 1).FirstOrDefault().PrescriptionScheduleId 
        //        //                       == usr.ReferenceToId).FirstOrDefault().Advice 
        //        //                       = "1";
            
        //    _db.Prescriptions.Add(newPres);
        //        _db.SaveChanges();

        //    return RedirectToAction("Apointments");
        //}

        public ActionResult AdvicePrescriptionForm(int? AppointmentID)
        {
            ViewBag.ScheduleID = AppointmentID;
            ViewBag.Drugs = _db.Drugs.ToList();
            var model = new AdvicePrescriptionViewModel
            {
                Prescriptions = new List<PrescriptionViewModel> { new PrescriptionViewModel() }
            };
            return View(model);
        }

        [HttpPost]
        public ActionResult AdvicePrescriptionForm(AdvicePrescriptionViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Save advice to the database.
                var advice = new Advice
                {
                    AdviceText = model.AdviceText,
                    ScheduleId = model.ScheduleID
                };
                _db.Advices.Add(advice);
                _db.SaveChanges();

                // Save prescriptions to the database.
                foreach (var prescription in model.Prescriptions)
                {
                    if (prescription != null && prescription.DrugName != null)
                    {
                        Drug drg = (from d in _db.Drugs
                                    where d.DrugName == prescription.DrugName
                                    select d).FirstOrDefault();
                        if (drg != null)
                        {
                            var physicianPrescription = new Prescription
                            {
                                PrescriptionAdviceId = advice.AdviceId,
                                DrugID = drg.DrugId,
                                Description = prescription.DrugDescription
                            };
                            _db.Prescriptions.Add(physicianPrescription);
                        }
                    }
                }
                _db.SaveChanges();
               
                // Redirect to a success page or another action
                return RedirectToAction("Index", "Physician");
            }
;
            return RedirectToAction("Index", "Physician");
        }


        public ActionResult RequestDrug()
        {
            return View();
        }

        [HttpPost]
        public ActionResult RequestDrug([Bind(Include = "RequestDescription")] DrugRequest drugRequest)
        {
            IBMCAS.Models.CurrentUserModel usr = Session["CurrentUser"] as IBMCAS.Models.CurrentUserModel;
            DrugRequest drugRequest2 = new DrugRequest();
            drugRequest2.RequestDescription = drugRequest.RequestDescription;
            drugRequest2.RequestPhysicianId = (int)usr.ReferenceToId;
            drugRequest2.RequestedDate = DateTime.Now.Date;
            _db.DrugRequests.Add(drugRequest2);
            _db.SaveChanges();
            return View("Index");

        }

    }
}