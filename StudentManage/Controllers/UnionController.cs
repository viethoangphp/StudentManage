using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StudentManage.BUS;
using StudentManage.Models;
namespace StudentManage.Controllers
{
    public class UnionController : BaseController
    {
        // GET: Union
        public ActionResult Index()
        {
            var list = new UnionBUS().GetListAll();
            return View(list);
        }
        public JsonResult Insert(UserModel model)
        {
            if(ModelState.IsValid)
            {
                // tạo 1 user bên bảng user
                model.password = model.studentCode;
                model.groupID = 2;
                model.positionID = 2;
                var userID = new UserBUS().Insert(model);
                // tạo sổ cho user ms thêm 
                var unionBook = new UnionModel();
                unionBook.create_by = (int)Session["USER_ID"];
                unionBook.userID = userID;
                unionBook.status = 1;
                var returnDate = Request.Form["returnDate"];
                unionBook.returnDate = DateTime.ParseExact(returnDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                var unionBUS = new UnionBUS().Insert(unionBook);
                return Json("true", JsonRequestBehavior.AllowGet);
            }

            return Json("false",JsonRequestBehavior.AllowGet);
        }
        public JsonResult View(int id)
        {
            var item = new UnionBUS().GetUnionById(id);
            if(item != null)
            {
                return Json(item, JsonRequestBehavior.AllowGet);
            }
            return Json(false, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetData()
        {
            var list = new UnionBUS().GetListAll();
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ChangeStatus(int id)
        {
            var isChange = new UnionBUS().ChangeStatus(id);
            if(isChange) 
                return Json(true,JsonRequestBehavior.AllowGet);
            return Json(false, JsonRequestBehavior.AllowGet);
        }
        public JsonResult InsertExcel(UserExcelModel model)
        {
            if (ModelState.IsValid)
            {
                var isInsert = new UserBUS().InsertFromExcel(model);
                if (isInsert > 0)
                {
                    var book = new UnionModel();
                    book.create_At = DateTime.Now;
                    book.create_by = (int)Session["USER_ID"];
                    book.userID = isInsert;
                    book.status = 1;
                    book.returnDate = DateTime.Now;
                    var result = new UnionBUS().Insert(book);
                    if(result != 0)
                        return Json(true, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(false, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(false, JsonRequestBehavior.AllowGet);
        }
    }
}