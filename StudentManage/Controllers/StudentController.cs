using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StudentManage.BUS;
using StudentManage.Library;
using StudentManage.Models;

namespace StudentManage.Controllers
{
    public class StudentController : BaseController
    {
        // GET: Student
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public JsonResult Insert(UserModel model)
        {
            return Json("Yes", JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Update(UserModel model)
        {
            ModelState.Remove("birthDay");
            ModelState.Remove("joinDate");
            var errors = ModelState.Where(x => x.Value.Errors.Count > 0).Select(x => new { x.Key, x.Value.Errors }).ToArray();//test
            if (ModelState.IsValid)
            {
                if (model.studentCode.Trim().Length <= 10)
                {
                    EmailService checkMail = new EmailService();
                    if (checkMail.IsValid(model.email.Trim()))
                    {
                        model.birthDay = DateTime.ParseExact(model.birthDayString, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                        model.joinDate = model.joinDate;
                        model.address = model.address == null ? "" : model.address;

                        var classModel = new FacultyBUS().GetClassModelByName(model.className);
                        if (classModel == null)
                        {
                            ClassModel classItem = new ClassModel()
                            {
                                className = model.className,
                                facultyID = model.facultyID,
                                status = 1
                            };
                            var classId = new FacultyBUS().InsertClass(classItem);
                            model.classID = classId;
                        }
                        else
                        {
                            model.classID = classModel.classID;
                        }
                        var userID = new UserBUS().Update(model);

                        if (userID > 0)
                        {
                            return Json(userID, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            return Json("dup", JsonRequestBehavior.AllowGet);
                        }
                    }
                    else
                    {
                        return Json("errorEmail", JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    return Json("errorStudentCode", JsonRequestBehavior.AllowGet);
                }
            }
            return Json("false", JsonRequestBehavior.AllowGet);
        }
        //    return Json("Yes",JsonRequestBehavior.AllowGet);
        //}
    }
}