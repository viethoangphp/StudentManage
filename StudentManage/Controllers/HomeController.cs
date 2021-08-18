using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StudentManage.Controllers
{
    public class HomeController : BaseController
    {
        // GET: Home
        public ActionResult Index()
        {
            return RedirectToAction("Index","Union");
        }
        public ActionResult Footer()
        {
            return PartialView();
        }
    }
}