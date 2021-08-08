﻿using StudentManage.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
namespace StudentManage.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public JsonResult Index(LoginModel model)
        {
            var isLogin = new LoginModel().CheckLogin(model.username, model.password);
            if(isLogin != -1)
            {
                Session.Add("USER_ID", isLogin);
                return Json("true", JsonRequestBehavior.AllowGet);
            }    
            return Json("false", JsonRequestBehavior.AllowGet);
        }
        public ActionResult Logout()
        {
            Session.RemoveAll();
            return RedirectToAction("Login", "Index");
        }
    }
}