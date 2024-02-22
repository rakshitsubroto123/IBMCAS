using IBMCAS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace IBMCAS.Controllers
{
    public class AppointmentRequestController : Controller
    {
        Models.IBMCASDBEntities2 _db = new Models.IBMCASDBEntities2();
        // GET: AppointmentRequest
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AppointmentRequest appointment = _db.AppointmentRequests.Where(a => a.AppointmentRequestToken == id).SingleOrDefault();
            if (appointment == null)
            {
                return HttpNotFound();
            }
            return PartialView(appointment);
        }
    }
}