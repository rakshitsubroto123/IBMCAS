using IBMCAS.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IBMCAS.Controllers
{
    [Authorize(Roles = "PATIENT")]
    public class PatientController : Controller
    {
        Models.IBMCASDBEntities2 _db = new Models.IBMCASDBEntities2();
        // GET: Patient
        public ActionResult Index()
        {
            CurrentUserModel cur = Session["CurrentUser"] as CurrentUserModel;
            return View(_db.Appointments.Where(a => a.PatientID == cur.ReferenceToId).ToList());
        }

        public ActionResult BookAppointment()
        {
            ViewBag.PhysicianList = _db.Physicians.ToList();
            return View();
        }
        [HttpPost]
        public ActionResult BookAppointment([Bind(Include = "PhysicianID, DateRequested")] Appointment appointment)
        {
            CurrentUserModel cur = Session["CurrentUser"] as CurrentUserModel;
            Appointment appointment1 = new Appointment();
            appointment1.PatientID = (int)cur.ReferenceToId;
            appointment1.isVisited = 0;
            appointment1.PhysicianID = appointment.PhysicianID;
            appointment1.DateCreated = DateTime.Now.Date;
            appointment1.DateRequested = appointment.DateRequested;
            appointment1.AppointmentToken = DateTime.Now.Year.ToString() + DateTime.Now.TimeOfDay.ToString("hhmmss");
            _db.Appointments.Add(appointment1);
            _db.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}