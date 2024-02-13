﻿using IBMCAS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace IBMCAS.Controllers
{
    public class UserCredentialController : Controller
    {
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(UserModel user)
        {
            if (ModelState.IsValid)
            {
                Models.IBMCASDBEntities1 _db = new Models.IBMCASDBEntities1();
                UserCred usr = _db.UserCreds.SingleOrDefault(dbusr => dbusr.UserName.ToLower()
                == user.UserName.ToLower() && dbusr.UserPassword == user.UserPassword);

                if (usr != null)
                {
                    FormsAuthentication.SetAuthCookie(usr.UserName, false);
                    CurrentUserModel cusr = new CurrentUserModel();
                    cusr.UserName = usr.UserName;
                    cusr.ReferenceToId = usr.UserReferneceToID;
                    cusr.UserID = usr.UserID;
                    cusr.Role = usr.UserRole;

                    if (usr.UserRole == Role.PHYSICIAN.ToString())
                    {
                        cusr.UserName = _db.Physicians.Find(usr.UserReferneceToID).PhysicianName;
                    }

                    Session["CurrentUser"] = cusr;

                    return RedirectToAction("Index", usr.UserRole);
                }
            }
            ModelState.AddModelError("", "invalid Entry of username and password");
            return View();
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }
    }
}