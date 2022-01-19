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
            UserUpdateModel model = new UserBUS().GetUpdateUserInfo(id);
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
        public ActionResult Insert(UserInsertModel model)
        {
            model.classID = 1; //Hardcoded class to 18DTHD1
            //model.facultyID = 1;
            //Check position and assign suitable group
            model.groupID = model.positionID;
            //switch (model.positionID)
            //{
            //    case 1:
            //        model.groupID = 1;
            //}
            if (ModelState.IsValid)
            {
                int result = new UserBUS().InsertUserByClass(model);
                if (result != -1)
                {
                    TempData["SUCCESS"] = "Thêm người dùng thành công";
                    return RedirectToAction("Index");
                }
                TempData["ERROR"] = "Đã có lỗi xảy ra. Vui lòng thử lại";
                return RedirectToAction("Index");
            }
            TempData["ERROR"] = "Dữ liệu không hợp lệ. Vui lòng kiểm tra lại";
            return RedirectToAction("Index");
        }
        //Update user
        [HttpPost]
        public ActionResult Update(UserUpdateModel model)
        {
            model.groupID = model.positionID;
            int result = new UserBUS().UpdateUser(model);
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
            int result = new UserBUS().DeleteUser(id);
            if (result == 0)
            {
                TempData["ERROR"] = "Không tìm thấy người dùng này";
                return RedirectToAction("Index");
            }
            else if (result == -1)
            {
                TempData["ERROR"] = "Lỗi hệ thống";
                return RedirectToAction("Index");
            }
            TempData["SUCCESS"] = "Xóa thành công";
            return RedirectToAction("Index");
        }
        #endregion
    }
}