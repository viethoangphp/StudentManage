using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StudentManage.Models;

namespace StudentManage.Controllers
{
    public class StudentController : BaseController
    {
        // GET: Student
        [HttpPost]
        public JsonResult Insert(UserModel model)
        {
            return Json("Yes",JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Update(UserModel model)
        {
            return Json("Yes",JsonRequestBehavior.AllowGet);
        }
    }
}