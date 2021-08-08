using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StudentManage.BUS;
using StudentManage.Models;
namespace StudentManage.Controllers
{
    public class ProfileController : BaseController
    {
        // GET: Profile
        public ActionResult Index()
        {
            var userID = (int)Session["USER_ID"];
            var user = new UserBUS().GetUserByID(userID);
            return View(user);
        }
    }
}