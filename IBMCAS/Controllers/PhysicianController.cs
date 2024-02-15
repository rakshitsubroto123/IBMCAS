using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IBMCAS.Controllers
{
    [Authorize(Roles = "PHYSICIAN")]

    public class PhysicianController : Controller
    {
        Models.IBMCASDBEntities1 _db = new Models.IBMCASDBEntities1();

        // GET: Physician
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Apointments()
        {
            return View(_db.Appointments.Where(q =>q.ScheduledDate == DateTime.Now.Date && q.isVisited == 1 && q.Advice == null));
        }
    }
}