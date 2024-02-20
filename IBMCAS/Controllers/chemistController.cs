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
    public class chemistController : Controller
    {
        Models.IBMCASDBEntities2 _db = new Models.IBMCASDBEntities2();
        // GET: chemist
       public ActionResult DrugIndex()
        {
            return View(_db.Drugs.ToList());
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CreateDrug()
        {
            ViewBag.SupplierId = _db.Suppliers.ToList();
            return View();
        }

        //public ActionResult DeleteDrug([Bind(Include = "DrugName, DrugQuantity, DrugExripyDate, DrugDescription, SupplierId ")] Drug drug)
        //{
        //    Drug newDrug = new Drug();
        //    newDrug.DrugName = drug.DrugName;
        //    newDrug.DrugQuantity = drug.DrugQuantity;
        //    newDrug.DrugExripyDate = drug.DrugExripyDate;
        //    newDrug.DrugDescription = drug.DrugDescription;
        //    newDrug.SupplierId = drug.SupplierId;
        //    _db.Drugs.Add(newDrug);
        //    return View("DrugIndex", _db.Drugs.ToList());
        //}



        public ActionResult DrugReuest()
        {
            return View(_db.DrugRequests.ToList());
        }
    }
}