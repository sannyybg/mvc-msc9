﻿using muscshop.Context;
using muscshop.filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace muscshop.Controllers
{
    [Authorize(Roles = "Admin")]
    [SessionFilter]
    public class UserController : Controller
    {

        StoreContext _storeContext = new StoreContext();

        public ActionResult Index()
        {
            
            var users = _storeContext.Users.Include("Roles");

            return View(users);
            
        }

        
        public ActionResult ChangeRole(int? id)
        {
            var user = _storeContext.Users.Include("Roles").Where(x => x.UserId == id).FirstOrDefault();

            var roleslist = user.Roles.Select(x => x.RoleName).ToList();

            if (roleslist.Contains("Manager"))
            {
                user.Roles = _storeContext.Roles.Where(x => x.RoleName == "Customer").ToList();
                _storeContext.SaveChanges();

                return RedirectToAction("index");
            }

            user.Roles = _storeContext.Roles.Where(x => x.RoleName == "Manager").ToList();
            _storeContext.SaveChanges();
            return RedirectToAction("index");
        }

        public ActionResult Search(string parameter)
        {
            var user = _storeContext.Users.Where(x => x.Username.ToLower().Contains(parameter.ToLower()));

            return View("index", user);
        }
    }
}