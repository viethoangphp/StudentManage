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
    public class ProfileController : BaseController
    {
        // GET: Profile
        protected UserBUS modelBUS = new UserBUS();
        public ActionResult Index()
        {
            var userID = (int)Session["USER_ID"];
            var user = modelBUS.GetUserByID(userID);
            return View(user);
        }
        public JsonResult ChangePassword(ChangePasswordModel model)
        {
            if(ModelState.IsValid)
            {
                var userID = (int)Session["USER_ID"];
                var isChange = modelBUS.ChangePassword(userID, model.passwordOld,model.passwordNew);
                if(isChange)
                     return Json("true", JsonRequestBehavior.AllowGet);
            }
            return Json("false", JsonRequestBehavior.AllowGet);
        }
        public JsonResult UpdateProfile(UserModel user)
        {
            ModelState.Remove("studentCode");
            if(ModelState.IsValid)
            {
                user.birthDay = DateTime.ParseExact(user.birthDayString, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                user.userID = (int)Session["USER_ID"];
                var isUpdate = modelBUS.UpdateProfile(user);
                if (isUpdate) return Json("true", JsonRequestBehavior.AllowGet);
                return Json("false", JsonRequestBehavior.AllowGet);
            }
            return Json("false", JsonRequestBehavior.AllowGet);

        }
    }
}