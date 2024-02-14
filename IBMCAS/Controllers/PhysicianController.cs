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
        // GET: Physician
        public ActionResult Index()
        {
            return View();
        }
    }
}