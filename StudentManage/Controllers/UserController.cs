using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Models.EntityModel;
using StudentManage.BUS;
using StudentManage.Models;

namespace StudentManage.Controllers
{
    public class UserController : BaseController
    {
        #region Get user data
        // Main view
        public ActionResult Index()
        {
            return View();
        }
        //Partial View List User
        public ActionResult ShowUserList()
        {
            return PartialView(new UserBUS().GetListUser());
        }
        //Partial View List Position
        public ActionResult GetListPosition()
        {
            return PartialView(new UserBUS().GetListPosition());
        }
        //Get user by id
        public JsonResult GetUserByID(int id)
        {
            UserModel model = new UserBUS().GetUserByID(id);
            model.birthDayString = model.birthDay != null ? ((DateTime)model.birthDay).ToString("dd/MM/yyyy") : null;
            return Json(model);
        }
        //Partial View List Group Name
        public ActionResult GetListGroup()
        {
            return PartialView(new UserBUS().GetListGroup());
        }
        #endregion

        #region Modify user data
        //Add user
        [HttpPost]
        public ActionResult Insert(UserModel model)
        {
            int result = new UserBUS().Insert(model);
            if(result == 1)
            {
                TempData["SUCCESS"] = "Thêm người dùng thành công";
                return RedirectToAction("Index");
            }
            TempData["ERROR"] = "Đã có lỗi xảy ra. Vui lòng thử lại";
            return RedirectToAction("Index");
        }
        //Update user
        [HttpPost]
        public ActionResult Update(UserModel model)
        {
            int result = new UserBUS().Update(model);
            if (result == 1)
            {
                TempData["SUCCESS"] = "Cập nhật người dùng thành công";
                return RedirectToAction("Index");
            }
            TempData["ERROR"] = "Đã có lỗi xảy ra. Vui lòng thử lại";
            return RedirectToAction("Index");
        }
        //Delete user
        [HttpPost]
        public ActionResult Delete(int id)
        {
            UserModel user = new UserBUS().GetUserByID(id);
            if (user == null)
            {
                TempData["ERROR"] = "Không tìm thấy người dùng này";
                return RedirectToAction("Index");
            }
            //Set status here >>>
            TempData["MESSAGE"] = "Not inplement yet";
            return RedirectToAction("Index");
        }
        #endregion
    }
}