using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using StudentManage.BUS;
using StudentManage.Library;
using StudentManage.Models;
namespace StudentManage.Controllers
{
    [UserAuthorze]
    public class ProfileController : Controller
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
                if(model.passwordNew.Equals(model.confirmPassword))
                {
                    var userID = (int)Session["USER_ID"];
                    model.passwordOld = HashPassword.HashSHA256(model.passwordOld, new SHA256CryptoServiceProvider());
                    model.passwordNew = HashPassword.HashSHA256(model.passwordNew, new SHA256CryptoServiceProvider());
                    var isChange = modelBUS.ChangePassword(userID, model.passwordOld, model.passwordNew);
                    if (isChange)
                        return Json("true", JsonRequestBehavior.AllowGet);
                    return Json("passwordError", JsonRequestBehavior.AllowGet);
                }
                return Json("confirmError", JsonRequestBehavior.AllowGet);
            }
            return Json("false", JsonRequestBehavior.AllowGet);
        }
        public JsonResult UpdateProfile(UserModel user)
        {
            ModelState.Remove("studentCode");
            ModelState.Remove("classID");
            ModelState.Remove("fullname");
            ModelState.Remove("facultyID");
            ModelState.Remove("joinDate");
            ModelState.Remove("className");
            if(ModelState.IsValid)
            {
                EmailService email = new EmailService();
                if (email.IsValid(user.email.Trim()))
                {
                    user.birthDay = DateTime.ParseExact(user.birthDayString, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    user.userID = (int)Session["USER_ID"];
                    var isUpdate = modelBUS.UpdateProfile(user);
                    if (isUpdate) return Json("true", JsonRequestBehavior.AllowGet);
                    return Json("false", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json("errorEmail", JsonRequestBehavior.AllowGet);
                }
                   
            }
            return Json("false", JsonRequestBehavior.AllowGet);

        }
        //-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
        // VietNam Provinces - DB Provinces (from API)
        public JsonResult GetAllCities()
        {
            var cities = new UnionBUS().GetAllCities();
            return Json(cities, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetDistrictsByCityID(int cityID)
        {
            var districts = new UnionBUS().GetDistrictsByCityID(cityID);
            return Json(districts, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetWardsByDistrictID(int districtID)
        {
            var wards = new UnionBUS().GetWardsByDistrictID(districtID);
            return Json(wards, JsonRequestBehavior.AllowGet);
        }
    }
}