﻿using IBMCAS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace IBMCAS.Controllers
{
    public class AppointmentController : Controller
    {
        Models.IBMCASDBEntities2 _db = new Models.IBMCASDBEntities2();
        // GET: Appointment
        public ActionResult Index()
        {
            return View();
        }

        [ChildActionOnly]
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Appointment appointment = _db.Appointments.Where(a => a.AppointmentToken == id).SingleOrDefault();
            if (appointment == null)
            {
                return HttpNotFound();
            }

            AppointmentViewModel viewModel = new AppointmentViewModel();
            viewModel.appointment = appointment;
            viewModel.advice = new Advice();
            viewModel.prescriptions = new List<Prescription>();
            viewModel.advice = _db.Advices.Where(a => a.ScheduleId == appointment.AppointmentID).SingleOrDefault();
            if (viewModel.advice != null)
            {
                viewModel.prescriptions = _db.Prescriptions.Where(p => p.PrescriptionAdviceId == viewModel.advice.AdviceId).ToList();
            }
            return PartialView(viewModel);
        }
    }
}