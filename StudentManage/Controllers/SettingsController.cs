using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using StudentManage.Models;
using StudentManage.BUS;

namespace StudentManage.Controllers
{
    public class SettingsController : BaseController
    {
        // GET: Settings
        public ActionResult Index()
        {
            EmailSettingsModel model = new EmailSettingsModel();
            foreach(PropertyInfo property in model.GetType().GetProperties())
            {
                property.SetValue(model, ConfigBUS.GetConfigByKey(property.Name));
            }
            return View(model);
        }
        [HttpPost]
        public ActionResult Save(EmailSettingsModel model)
        {
            foreach(PropertyInfo propertyinfo in model.GetType().GetProperties())
            {
                int st = ConfigBUS.SetConfigByKey(propertyinfo.Name, propertyinfo.GetValue(model).ToString());
                if(st == 0)
                {
                    TempData["ERROR"] = "Đã có lỗi xảy ra\nVui lòng thử lại";
                    return RedirectToAction("Index");
                }
            }
            TempData["MESSAGE"] = "Đã lưu thành công";
            return RedirectToAction("Index");
        }
    }
}