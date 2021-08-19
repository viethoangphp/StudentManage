using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StudentManage.BUS;
using StudentManage.Library;
using StudentManage.Models;
using System.IO;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Reflection;
using System.Drawing;
using System.Threading;

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

                            // Send mail trong thread, ko catch exception vì send mail optional, có thể fail
                            Thread sendMail = new Thread(() => {
                                try
                                {
                                    var emailService = new EmailService();
                                    emailService.Send(user.email, "<div style='width: 750px; padding: 10px ;margin: 0 auto; font-size: 1.2rem;'> <div style='border: 3px solid #222; padding: 1px; background-color: #fb0000cf;'> <div style='border: 3px solid #222; padding: 20px;background: #fdf6ec;'> <div style='text-align: center;'> <h1 style='font-size: 1.5rem; margin-top: 0; margin-bottom: 5px; text-transform: uppercase;'>Đoàn trường đại học công nghệ tp.hcm</h1> <span>-----o0o-----</span> <h3 style='font-size: 1.6rem; margin-top: 9px; text-transform: uppercase;'>biên nhận hồ sơ đoàn viên</h3> </div><div style='text-align: end;'> <p style='margin: 0;'>Sổ " + unionBUS + "/" + DateTime.Now.Year + "</p></div><div style='display:flex;'> <p style='margin: 9px 0;'>Họ và tên:</p><p style='margin: 9px 0;'>&emsp;" + user.fullname + "</p></div><div style='display:flex;'> <div style='display:flex; width:50%'> <p style='margin: 9px 0;'>Ngày sinh:</p><p style='margin: 9px 0;'>&emsp;" + String.Format("{0:dd/MM/yyyy}", user.birthDay) + "</p></div><div style='display:flex; width:50%'> <p style='margin: 9px 0;'>MSSV:</p><p style='margin: 9px 0;'>&emsp;" + user.studentCode + "</p></div></div><div style='display:flex;'> <div style='display:flex; width:50%'> <p style='margin: 9px 0;'>Lớp:</p><p style='margin: 9px 0;'>&emsp;" + user.className + "</p></div><div style='display:flex; width:50%'> <p style='margin: 9px 0;'>Khoa:</p><p style='margin: 9px 0;'>&emsp;" + user.facultyName + "</p></div></div><div style='margin-top: 20px;'> <p style='margin: 7px 0;'><b style='text-decoration:underline;'>Lưu ý:</b> Giữ gìn biên nhận cẩn thận, mang theo biên nhận khi rút sổ.</p><p style='margin: 7px 0;'>Vui lòng rút sổ trước ngày:" + String.Format("{0:dd/MM/yyyy}", unionBook.returnDate) + "</p><p style='margin: 7px 0;'>Thắc mắc xin liên hệ <b>(028)-3512 0293</b> hoặc <b>doanthanhnien@hutech.edu.vn</b> </p></div><div style='text-align: end;'> <p>TP.Hồ Chí Minh, ngày ... tháng ... năm 20..</p><div style='width: 20%; margin-left: auto; margin-right: 80px; text-align: center;'> <p style='margin: 5px 0; font-weight: 600;'>Người nhận</p><p style='margin: 14px 0;'></p><p style='margin: 14px 0;'></p></div></div></div></div></div>");
                                }
                                catch
                                {
                                    // Do nothing
                                }
                            });
                            sendMail.IsBackground = true;
                            sendMail.Start();

                            // Return data
                            return Json(unionID, JsonRequestBehavior.AllowGet);
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
            int total = list.Count;
            List<UserExcelModel> listError = new List<UserExcelModel>();
            // Convert list model excel to model union book
            var listUnionBook = new List<UnionModel>();
            foreach (var item in list)
            {
                // Check validate data
                if (string.IsNullOrEmpty(item.fullname) || string.IsNullOrEmpty(item.email) || string.IsNullOrEmpty(item.studentCode) ||
                    string.IsNullOrEmpty(item.phone) || string.IsNullOrEmpty(item.facultyName) || string.IsNullOrEmpty(item.className) ||
                    string.IsNullOrEmpty(item.unionID))
                {
                    listError.Add(item);
                    error++;
                    continue;
                }

                // Check union number id
                int num = -1;
                if (!int.TryParse(item.unionID, out num))
                {
                    listError.Add(item);
                    error++;
                    continue;
                }

                // Convert to UnionModel
                var isInsert = new UserBUS().InsertFromExcel(item);
                if (isInsert <= 0)
                {
                    listError.Add(item);
                    error++;
                    continue;
                }

                var book = new UnionModel();
                book.create_At = DateTime.Now;
                book.create_by = (int)Session["USER_ID"];
                book.userID = isInsert;
                book.unionID = item.unionID;
                book.status = 1;
                book.returnDate = DateTime.Now;

                listUnionBook.Add(book);
            }

            // Insert list data to db
            var listFail = UnionBUS.SaveListUnionBook(listUnionBook);

            // Get list error
            error += listFail.Count;
            foreach (var itemFail in listFail)
            {
                var modelFail = list.Where(x => !string.IsNullOrEmpty(x.unionID) && x.unionID.Trim().Equals(itemFail)).First();
                listError.Add(modelFail);
            }

            // Return data
            Session["ERROR_LIST"] = listError;
            return Json(new
            {
                error,
                success = total - error
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

        //Export Excel
        public ActionResult ExportExcel(int classId, string unionId, int status, int facultyId, int semester)
        {
            //---=== Init ===---
            ExcelPackage pkg = new ExcelPackage();
            ExcelWorksheet sheet = pkg.Workbook.Worksheets.Add("Đoàn viên");
            string[] listColName = new string[] { "Mã sổ đoàn", "Họ và tên", "MSSV", "Lớp", "Khoa", "Ngày nộp", "Ngày rút", "Trạng thái" };
            string[] selectProperties = new string[] { "unionID", "fullname", "studentCode", "className", "facultyName", "create_At", "update_At", "status", };
            //---=== Configure ===---
            //Header
            ExcelRange header = sheet.Cells[1, 1, 1, listColName.Length];
            header.Merge = true;
            header.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            header.Value = "Danh sách đoàn viên";
            header.Style.Font.Size = 24;
            //Column name
            int posRow = 2; //Start position of row
            int posCol = 1; //Start position of column
            foreach (string name in listColName)
            {
                ExcelRange colTitle = sheet.Cells[posRow, posCol];
                colTitle.Value = name;
                colTitle.Style.Font.Bold = true;
                colTitle.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                posCol++;
            }
            //Add sorting to column name
            ExcelRange colTitleRow = sheet.Cells[2, 1, 2, listColName.Length];
            colTitleRow.AutoFilter = true;
            
            //Column data
            List<UnionModel> listUnion = new UnionBUS().GetUnionBookByCondition(classId, unionId, status, facultyId, semester);
            posRow++; // Set to 3 (next row)
            foreach (UnionModel item in listUnion)
            {
                posCol = 1;
                foreach (PropertyInfo propertyInfo in item.GetType().GetProperties())
                {
                    ExcelRange cellData = sheet.Cells[posRow, posCol];
                    cellData.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    if (selectProperties.Contains(propertyInfo.Name))
                    {
                        //Check if value is null
                        string value = null;
                        if (propertyInfo.GetValue(item) != null)
                        {
                            value = propertyInfo.GetValue(item).ToString();
                        }
                        //Specific data for returnDate and status
                        if (propertyInfo.Name.Contains("update_At"))
                        {
                            if (string.IsNullOrEmpty(value)) { value = "Chưa rút sổ"; }
                        }
                        if (propertyInfo.Name.Contains("status"))
                        {
                            if(value == "1") { value = "Đã nộp"; } else { value = "Đã rút"; }
                        }
                        //Bind it to excel cell
                        cellData.Value = value;
                        posCol++;
                    }
                }
                posRow++;
            }
            //Auto fit all row
            ExcelRange fitRow = sheet.Cells[2, 1, posRow, listColName.Length];
            fitRow.AutoFitColumns();
            //Send back to client for download
            MemoryStream stream = new MemoryStream();
            pkg.SaveAs(stream);
            stream.Position = 0;

            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet.main+xml", DateTime.Now.ToString() + ".xlsx");
        }
        //-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-= HIGHLIGHT ERROR EXCEL =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
        public ActionResult DownloadErrorHighlight()
        {
            if (Session["ERROR_LIST"] == null) return RedirectToAction("Index","Union");
            List<UserExcelModel> listError = (List<UserExcelModel>)Session["ERROR_LIST"];
            //---=== Init ===---
            ExcelPackage pkg = new ExcelPackage();
            ExcelWorksheet sheet = pkg.Workbook.Worksheets.Add("Đoàn viên");
            string[] listColName = new string[] {"Mã sổ đoàn", "Họ Tên", "MSSV", "Số điện thoại", "Email", "Lớp", "Khoa"};
            //---=== Configure ===---
            //Column name
            int posRow = 1; //Start position of row
            int posCol = 1; //Start position of column
            foreach (string name in listColName)
            {
                ExcelRange colTitle = sheet.Cells[posRow, posCol];
                colTitle.Value = name;
                colTitle.Style.Font.Bold = true;
                colTitle.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                posCol++;
            }
            //Column data
            posRow++; // Set to 2 (next row)
            Color highlight = ColorTranslator.FromHtml("#FFFF00"); //Highlight color
            foreach (UserExcelModel item in listError)
            {
                posCol = 1;
                foreach (PropertyInfo propertyInfo in item.GetType().GetProperties())
                {
                    ExcelRange cellData = sheet.Cells[posRow, posCol];
                    cellData.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    if (propertyInfo.GetValue(item) == null) 
                    {
                        cellData.Value = null;
                    }
                    else //Add invaild condition here
                    {
                        cellData.Value = propertyInfo.GetValue(item);
                    }
                    posCol++;
                }
                posRow++;
            }
            //Auto fit all row
            ExcelRange fitRow = sheet.Cells[1, 1, posRow, listColName.Length];
            fitRow.AutoFitColumns();
            //Add sorting to column name
            ExcelRange colTitleRow = sheet.Cells[1, 1, 1, listColName.Length];
            colTitleRow.AutoFilter = true;
            //Send back to client for download
            MemoryStream stream = new MemoryStream();
            pkg.SaveAs(stream);
            stream.Position = 0;

            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet.main+xml", "Error " + DateTime.Now.ToString() + ".xlsx");
        }
    }
}