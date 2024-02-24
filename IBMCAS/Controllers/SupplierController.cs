
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IBMCAS.Controllers
{
    public class SupplierController : Controller
    {
        Models.IBMCASDBEntities2 _db = new Models.IBMCASDBEntities2();

        // GET: Supplier

        public ActionResult Index()
        {
            IBMCAS.Models.CurrentUserModel usr = Session["CurrentUser"] as IBMCAS.Models.CurrentUserModel;
            return View(_db.PurchaseOrderHeaders.Where(q => q.SupplierId == usr.ReferenceToId).ToList());
        }
        public ActionResult MedicineOrdered()
        {
            IBMCAS.Models.CurrentUserModel usr = Session["CurrentUser"] as IBMCAS.Models.CurrentUserModel;
            return View(_db.PurchaseOrderProductLines.Where(q => q.PurchaseOrderId == usr.ReferenceToId).ToList());
        }

        public ActionResult OrderList(int id)
        {
            return View(_db.PurchaseOrderProductLines.Where(q=>q.PurchaseOrderId == id).ToList());
        }
    }
}