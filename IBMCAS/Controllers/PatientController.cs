using IBMCAS.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Runtime.ConstrainedExecution;
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
            return RedirectToAction("Dashboard");
        }

        public ActionResult Dashboard()
        {
            CurrentUserModel cur = Session["CurrentUser"] as CurrentUserModel;
            PatientDashBoardModel model = new PatientDashBoardModel();
            model.Appointments = _db.Appointments.Where(a => a.PatientID == cur.ReferenceToId).ToList();
            model.AppointmentRequests = _db.AppointmentRequests.Where(a => a.PatientID == cur.ReferenceToId).ToList();
            return View(model);
        }

        public ActionResult BookAppointment()
        {
            ViewBag.PhysicianList = _db.Physicians.ToList();
            return View();
        }
        [HttpPost]
        public ActionResult BookAppointment([Bind(Include = "PhysicianID, DateRequested")] AppointmentRequest appointment)
        {
            CurrentUserModel cur = Session["CurrentUser"] as CurrentUserModel;
            appointment.PatientID = (int)cur.ReferenceToId;
            appointment.DateCreated = DateTime.Now.Date;
            appointment.AppointmentRequestToken = DateTime.Now.Year.ToString() + DateTime.Now.TimeOfDay.ToString("hhmmss");
            _db.AppointmentRequests.Add(appointment);
            _db.SaveChanges();

            return RedirectToAction("Index");
        }
        public ActionResult RejectedAppointment()
        {
            CurrentUserModel cur = Session["CurrentUser"] as CurrentUserModel;
            return View(_db.AppointmentRequests.Where(a => a.PatientID == cur.ReferenceToId && a.status == -1).ToList());
        }

    }
}