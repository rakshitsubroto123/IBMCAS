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
        Models.IBMCASDBEntities1 _db = new Models.IBMCASDBEntities1();
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
        public ActionResult BookAppointment([Bind(Include = "PhysicianID, DateRequested" )] Appointment appointment)
        {
            CurrentUserModel cur = Session["CurrentUser"] as CurrentUserModel;
            appointment.PatientID = (int)cur.ReferenceToId;
            appointment.isVisited = 0;
            appointment.DateCreated = DateTime.Now.Date;
            appointment.AppointmentToken = DateTime.Now.Year.ToString() + DateTime.Now.TimeOfDay.ToString("hhmmss");
            _db.Appointments.Add(appointment);
            _db.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}