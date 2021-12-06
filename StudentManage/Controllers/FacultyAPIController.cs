using StudentManage.BUS;
using StudentManage.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace StudentManage.Controllers
{
    public class FacultyAPIController : ApiController
    {
        [HttpPost]
        public IHttpActionResult Index(DataTableModel model)
        {
            List<FacultyModel> listFac = new FacultyBUS().GetListFaculty().Skip(model.start).Take(model.length).ToList();
            int total = new FacultyBUS().GetListFaculty().Count();
            return Json(new
            {
                draw = model.draw,
                recordsTotal = total,
                recordsFiltered = total,
                data = listFac
            });
        }
        [HttpPost]
        public IHttpActionResult Insert(FacultyModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.phone.Length > 10) { return Json("invaildPhone");}
                int id = new FacultyBUS().InsertFaculty(model);
                if (id != 0)
                {
                    return Json(true);
                }
                else
                {
                    return Json(false);
                }
            }
            return Json(false);
        }
        [HttpPost]
        public IHttpActionResult Update(FacultyModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.phone.Length > 10) { return Json("invaildPhone"); }
                int st = new FacultyBUS().UpdateFaculty(model);
                if (st != 0)
                {
                    return Json(true);
                }
                else
                {
                    return Json(false);
                }
            }
            return Json(false);
        }
        [HttpPost]
        public IHttpActionResult Delete(int id)
        {
            if (ModelState.IsValid)
            {
                int st = new FacultyBUS().DeleteFaculty(id);
                if (st != 0)
                {
                    return Json(true);
                }
                else
                {
                    return Json(false);
                }
            }
            return Json(false);
        }
    }
}
