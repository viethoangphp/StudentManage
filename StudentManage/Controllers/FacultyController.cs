//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using System.Web.Mvc;
//using StudentManage.Models;
//using StudentManage.BUS;

namespace StudentManage.Controllers
{
    public class FacultyController : BaseController
    {
        // GET: Faculty
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public JsonResult GetData(DataTableModel model)
        {
            List<FacultyModel> listFac = new FacultyBUS().GetListFaculty().Skip(model.start).Take(model.length).ToList();
            int total = new FacultyBUS().GetListFaculty().Count();
            return Json(new
            {
                draw = model.draw,
                recordsTotal = total,
                recordsFiltered = total,
                data = listFac
            }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Get(int id)
        {
            return Json(new FacultyBUS().GetFacultyByID(id),JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Insert(FacultyModel model)
        {
            if (ModelState.IsValid)
            {
                if(model.phone.Length > 10) { return Json("invaildPhone", JsonRequestBehavior.AllowGet); }
                int id = new FacultyBUS().InsertFaculty(model);
                if(id != 0)
                {
                    return Json("true", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json("false", JsonRequestBehavior.AllowGet);
                }
            }
            return Json("false", JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Update(FacultyModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.phone.Length > 10) { return Json("invaildPhone", JsonRequestBehavior.AllowGet); }
                int st = new FacultyBUS().UpdateFaculty(model);
                if(st != 0)
                {
                    return Json("true", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json("false", JsonRequestBehavior.AllowGet);
                }
            }
            return Json("false", JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Delete(int id)
        {
            if (ModelState.IsValid)
            {
                int st = new FacultyBUS().DeleteFaculty(id);
                if(st != 0)
                {
                    return Json("true", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json("false", JsonRequestBehavior.AllowGet);
                }
            }
            return Json("false", JsonRequestBehavior.AllowGet);
        }
    }
}