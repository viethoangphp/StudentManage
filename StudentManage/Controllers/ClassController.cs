using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StudentManage.BUS;
using StudentManage.Models;

namespace StudentManage.Controllers
{
    public class ClassController : BaseController
    {
        // GET: Class
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public JsonResult GetData(DataTableModel model)
        {
            var result = new FacultyBUS().GetListClass(model.start,model.length);
            var listCls = result.Item2;
            var total = result.Item1;
            return Json(new
            {
                draw = model.draw,
                recordsTotal = total,
                recordsFiltered = total,
                data = listCls
            }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Get(int id)
        {
            return Json(new FacultyBUS().GetClassByID(id), JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Insert(ClassModel model)
        {
            if (ModelState.IsValid)
            {
                int id = new FacultyBUS().InsertClass(model);
                if (id > 0)
                {
                    return Json("true", JsonRequestBehavior.AllowGet);
                }
                else if(id == -1)
                {
                    return Json("dup", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json("false", JsonRequestBehavior.AllowGet);
                }
            }
            return Json("false", JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Update(ClassModel model)
        {
            if (ModelState.IsValid)
            {
                int st = new FacultyBUS().UpdateClass(model);
                if (st > 0)
                {
                    return Json("true", JsonRequestBehavior.AllowGet);
                }
                else if (st == -1)
                {
                    return Json("dup", JsonRequestBehavior.AllowGet);
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
                int st = new FacultyBUS().DeleteClass(id);
                if (st != 0)
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
        public ActionResult FacultyPartial()
        {
            return PartialView(new FacultyBUS().GetListFaculty());
        }
        public JsonResult Search(DataTableModel model, int facultyID = 0, string className = "")
        {
            var result = new FacultyBUS().GetListClassByCondition(model.start, model.length, facultyID, className);
            var list = result.Item2;
            var total = result.Item1;
            if (list != null)
                return Json(new
                {
                    draw = model.draw,
                    recordsTotal = total,
                    recordsFiltered = total,
                    data = list
                }, JsonRequestBehavior.AllowGet);
            return Json(false, JsonRequestBehavior.AllowGet);
        }
        public ActionResult List(int? id)
        {
            if (id == null || id <= 0) return RedirectToAction("Index");
            return View(new FacultyBUS().GetClassByID((int)id));
        }
        [HttpPost]
        public JsonResult List(DataTableModel model,int id)
        {
            List<UserModel> list = new UserBUS().GetListUserByClass(id).Skip(model.start).Take(model.length).ToList();
            int count = new UserBUS().GetListUserByClass(id).Count;
            return Json(new
            {
                draw = model.draw,
                recordsTotal = count,
                recordsFiltered = count,
                data = list
            }, JsonRequestBehavior.AllowGet);
        }
    }
}