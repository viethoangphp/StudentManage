using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StudentManage.BUS;
using StudentManage.Library;
using StudentManage.Models;
namespace StudentManage.Controllers
{
    public class UnionController : BaseController
    {
        // GET: Union
        public ActionResult Index()
        {
            var userID = (int)Session["USER_ID"];
            var posID = new UserBUS().GetUserByID(userID).positionID;
            if(posID != 1)
            {
                return View("Collaborator");
            }    
            return View();
        }
        public ActionResult Faculty()
        {
            var result = new FacultyBUS().GetListFaculty();
            return PartialView(result);
        }
        public JsonResult Insert(UserModel model)
        {
            ModelState.Remove("birthDay");
            if(ModelState.IsValid)
            {
                // tạo 1 user bên bảng user
               if(model.studentCode.Length <= 10)
               {
                    EmailService checkMail = new EmailService();
                    if(checkMail.IsValid(model.email))
                    {
                        model.password = model.studentCode;
                        model.groupID = 2;
                        model.positionID = 2;
                        model.birthDay = DateTime.ParseExact(model.birthDayString, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                        var classModel = new FacultyBUS().GetClassModelByName(model.className);
                        if (classModel == null)
                        {
                            ClassModel classItem = new ClassModel()
                            {
                                className = model.className,
                                facultyID = model.facultyID,
                                status = 1
                            };
                            var classId = new FacultyBUS().InsertClass(classItem);
                            model.classID = classId;
                        }
                        else
                        {
                            model.classID = classModel.classID;
                        }
                        var userID = new UserBUS().Insert(model);
                        if (userID > 0)
                        {
                            var user = new UserBUS().GetUserByID(userID);
                            // tạo sổ cho user ms thêm 
                            var unionBook = new UnionModel();
                            unionBook.create_by = (int)Session["USER_ID"];
                            unionBook.userID = userID;
                            unionBook.status = 1;
                            var returnDate = Request.Form["returnDate"];
                            unionBook.returnDate = DateTime.ParseExact(returnDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                            var unionBUS = new UnionBUS().Insert(unionBook);
                            var unionID = Convert.ToInt32(unionBUS);
                            var sendMail = new EmailService();
                            try
                            {
                                sendMail.Send(user.email, "<div style='width: 750px; padding: 10px ;margin: 0 auto; font-size: 1.2rem;'> <div style='border: 3px solid #222; padding: 1px; background-color: #fb0000cf;'> <div style='border: 3px solid #222; padding: 20px;background: #fdf6ec;'> <div style='text-align: center;'> <h1 style='font-size: 1.5rem; margin-top: 0; margin-bottom: 5px; text-transform: uppercase;'>Đoàn trường đại học công nghệ tp.hcm</h1> <span>-----o0o-----</span> <h3 style='font-size: 1.6rem; margin-top: 9px; text-transform: uppercase;'>biên nhận hồ sơ đoàn viên</h3> </div><div style='text-align: end;'> <p style='margin: 0;'>Sổ " + unionBUS + "/" + DateTime.Now.Year + "</p></div><div style='display:flex;'> <p style='margin: 9px 0;'>Họ và tên:</p><p style='margin: 9px 0;'>&emsp;" + user.fullname + "</p></div><div style='display:flex;'> <div style='display:flex; width:50%'> <p style='margin: 9px 0;'>Ngày sinh:</p><p style='margin: 9px 0;'>&emsp;" + String.Format("{0:dd/MM/yyyy}", user.birthDay) + "</p></div><div style='display:flex; width:50%'> <p style='margin: 9px 0;'>MSSV:</p><p style='margin: 9px 0;'>&emsp;" + user.studentCode + "</p></div></div><div style='display:flex;'> <div style='display:flex; width:50%'> <p style='margin: 9px 0;'>Lớp:</p><p style='margin: 9px 0;'>&emsp;" + user.className + "</p></div><div style='display:flex; width:50%'> <p style='margin: 9px 0;'>Khoa:</p><p style='margin: 9px 0;'>&emsp;" + user.facultyName + "</p></div></div><div style='margin-top: 20px;'> <p style='margin: 7px 0;'><b style='text-decoration:underline;'>Lưu ý:</b> Giữ gìn biên nhận cẩn thận, mang theo biên nhận khi rút sổ.</p><p style='margin: 7px 0;'>Vui lòng rút sổ trước ngày:" + String.Format("{0:dd/MM/yyyy}", unionBook.returnDate) + "</p><p style='margin: 7px 0;'>Thắc mắc xin liên hệ <b>(028)-3512 0293</b> hoặc <b>doanthanhnien@hutech.edu.vn</b> </p></div><div style='text-align: end;'> <p>TP.Hồ Chí Minh, ngày ... tháng ... năm 20..</p><div style='width: 20%; margin-left: auto; margin-right: 80px; text-align: center;'> <p style='margin: 5px 0; font-weight: 600;'>Người nhận</p><p style='margin: 14px 0;'></p><p style='margin: 14px 0;'></p></div></div></div></div></div>");
                                return Json(unionID, JsonRequestBehavior.AllowGet);
                            }
                            catch (Exception)
                            {
                               
                            }
                           
                        }
                        else
                        {
                            return Json("dup", JsonRequestBehavior.AllowGet);
                        }
                    }
                    else
                    {
                        return Json("errorEmail", JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    return Json("errorStudentCode", JsonRequestBehavior.AllowGet);
                }
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
        public JsonResult GetData(DataTableModel model)
        {
            var list = new UnionBUS().GetListAll(model.start,model.length);
            var total = new UnionBUS().getTotalRecord();
            return Json(new
            {
                draw = model.draw,
                recordsTotal =total,
                recordsFiltered = total,
                data = list
            }, JsonRequestBehavior.AllowGet) ;
        }
        public JsonResult ChangeStatus(int id)
        {
            var isChange = new UnionBUS().ChangeStatus(id);
            if(isChange) 
                return Json(true,JsonRequestBehavior.AllowGet);
            return Json(false, JsonRequestBehavior.AllowGet);
        }
        public JsonResult InsertExcel(List<UserExcelModel> list)
        {
            int error = 0;
            int success = 0;
             foreach(var item in list)
             {
                if (ModelState.IsValid)
                {
                    var isInsert = new UserBUS().InsertFromExcel(item);
                    if (isInsert > 0)
                    {
                        var book = new UnionModel();
                        book.create_At = DateTime.Now;
                        book.create_by = (int)Session["USER_ID"];
                        book.userID = isInsert;
                        book.status = 1;
                        book.returnDate = DateTime.Now;
                        var result = new UnionBUS().Insert(book);
                        if (result < 0) error++;
                        success++;
                    }
                    else
                    {
                        error++;
                    }
                }
                else
                {
                    error++;
                    continue;
                }
              
            }
            return Json(new { 
                error = error,
                success = success
            }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetClassData(DataTableModel model,int facultyId,int year)
        {
            var list = new UserBUS().GetListClassByCondition(facultyId, year);
            if (list != null)
                return Json(list, JsonRequestBehavior.AllowGet);
            return Json(false, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Search(DataTableModel model,int classId = 0,int status = 0,string unionId = "",int semester = 0,int facultyId = 0)
        {
            var result = new UnionBUS().GetUnionBookByCondition(model.start,model.length,classId,unionId,status,facultyId,semester);
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
        public ActionResult Print(int id)
        {
            var union = new UnionBUS().GetUnionById(id);
            return View(union);
        }
        public JsonResult SendEmail(int id)
        {
            try
            {
                var unionBUS = new UnionBUS();
                unionBUS.SendReturnEmail(id);
                return Json("true", JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json("true", JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult ListFacuty()
        {
            var result = new FacultyBUS().GetListFaculty();
            return PartialView(result);
        }
    }
}