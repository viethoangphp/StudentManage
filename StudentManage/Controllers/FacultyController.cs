using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StudentManage.Models;
using StudentManage.BUS;

namespace StudentManage.Controllers
{
    public class FacultyController : BaseController
    {
        // GET: Faculty
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult GetData(DataTableModel model)
        {
            //Pending code change 
            return Json(new
            {
                draw = model.draw,
                recordsTotal = 0,
                recordsFiltered = 0,
                data = ""
            }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Insert(FacultyModel model)
        {
            if (ModelState.IsValid)
            {
                int id = new FacultyBUS().InsertFaculty(model);
                if(id != 0)
                {
                    return Json(true, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(false, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(false, JsonRequestBehavior.AllowGet);
        }
        /*public JsonResult Update(FacultyModel model)
        {
            if (ModelState.IsValid)
            {
                int id = new FacultyBUS().UpdateFaculty(model);
                if(id != 0)
                {
                    return Json(true, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(false, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(false, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Delete(FacultyModel model)
        {
            if (ModelState.IsValid)
            {
                int st = new FacultyBUS().DeleteFaculty(model);
                if(st != 0)
                {
                    return Json(true, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(false, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(false, JsonRequestBehavior.AllowGet);
        }*/
    }
}