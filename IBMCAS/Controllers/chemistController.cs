using IBMCAS.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static System.Data.Entity.Infrastructure.Design.Executor;

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
            return RedirectToAction("DrugReuest");
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

        public ActionResult suppliersList()
        {
            return View(_db.Suppliers.ToList());
        }

        public ActionResult PurchaseOrder(int? id)
        {
            ViewBag.SupplierId = id;
            ViewBag.Drugs = _db.Drugs.ToList();
            var model = new PurchaseLine
            {
                pbdies = new List<pBody> { new pBody() }
            };
            return View(model);
        }

        [HttpPost]
        public ActionResult PurchaseOrder(PurchaseLine model)
        {
            if (ModelState.IsValid)
            {
                // Save advice to the database.
                var pheader = new PurchaseOrderHeader
                {
                    PODate = DateTime.Now.Date,
                    Note = model.Note,
                    SupplierId = model.SupplierId
                };
                _db.PurchaseOrderHeaders.Add(pheader);
                _db.SaveChanges();

                // Save prescriptions to the database.
                foreach (var pbody in model.pbdies)
                {
                    if (pbody != null && pbody.DrugName != null)
                    {
                        Drug drg = (from d in _db.Drugs
                                    where d.DrugName == pbody.DrugName
                                    select d).FirstOrDefault();
                        if (drg != null)
                        {
                            var popl = new PurchaseOrderProductLine
                            {
                                PurchaseOrderId = pheader.PurchaseOrderId,
                                DrugId = drg.DrugId,
                                ShortNote = pbody.ShortNote,
                                Qty = pbody.Qty
                            };
                            _db.PurchaseOrderProductLines.Add(popl);
                        }
                    }
                }
                _db.SaveChanges();

                // Redirect to a success page or another action
                return RedirectToAction("suppliersList", "chemist");
            }
            return RedirectToAction("Index", "chemist");
        }

    }
}