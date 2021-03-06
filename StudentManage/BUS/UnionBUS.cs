using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Models.DAO;
using Models.EntityModel;
using StudentManage.Library;
using StudentManage.Models;
namespace StudentManage.BUS
{
    public class UnionBUS
    {
        UnionDAO dao = new UnionDAO();

        public int Insert(UnionModel model)
        {
            return dao.Insert(ParseModel(model));
        }

        public int getTotalRecord()
        {
            return dao.getTotalRecord();
        }
        public void UpdateIsEmail(int id,int isEmail)
        {
             dao.UpdateIsEmail(id,isEmail);
        }
        public List<UnionModel> GetListAll(int start, int length)
        {
            var list = dao.GetListAll(start, length);
            var listUnion = new List<UnionModel>();
            foreach (var item in list)
            {
                UnionModel union = new UnionModel();
                union.id = item.ID;
                union.unionID = item.User1.StudentCode.Substring(0, 2) + "-" + item.NumID.ToString().PadLeft(5, '0');
                union.fullname = item.User1.FullName;
                union.studentCode = item.User1.StudentCode;
                union.className = item.User1.Class.Name;
                union.facultyName = item.User1.Class.Faculty.Name;
                union.create_At = item.Create_At;
                union.update_At = item.Update_At;
                union.create_at = String.Format("{0:dd/MM/yyyy h:mm tt}", item.Create_At);
                union.status = (int)item.Status;
                listUnion.Add(union);
            }
            return listUnion;
        }
        public UnionModel GetUnionById(int id)
        {
            var item = dao.getUnionBookById(id);
            if (item != null)
            {
                var union = new UnionModel();
                union.id = item.ID;
                union.userID = (int)item.UserID;
                union.cityId =(item.User1.CityID == null) ? 0: Convert.ToInt32(item.User1.CityID);
                union.districtId = (item.User1.DistrictID == null) ? 0 : Convert.ToInt32(item.User1.DistrictID);
                union.wardId = (item.User1.DistrictID == null) ? 0 : Convert.ToInt32(item.User1.WardID);
                union.unionID = item.User1.StudentCode.Substring(0, 2) + "-" + item.NumID.ToString().PadLeft(5, '0');
                union.fullname = item.User1.FullName.Trim();
                union.studentCode = item.User1.StudentCode.Trim();
                union.className = item.User1.Class.Name;
                union.facultyName = item.User1.Class.Faculty.Name;
                union.create_At = item.Create_At;
                union.status = (int)item.Status;
                union.birthDay = String.Format("{0:dd/MM/yyyy}", item.User1.Birthday);
                union.update_At = item.Update_At;
                union.ReturnDate = String.Format("{0:dd/MM/yyyy}", item.ReturnDate);
                union.UpdateAt = String.Format("{0:dd/MM/yyyy}", item.Update_At);
                union.isEmail = Convert.ToInt32(item.isEmail);
                union.JoinDate = String.Format("{0:dd/MM/yyyy}", item.User1.JoinDate);
                union.address = item.User1.Address;
                union.facultyId = item.User1.Class.FacutyID;
                union.gender = (int)item.User1.Gender;
                union.phone = item.User1.Phone;
                union.email = item.User1.Email;
                return union;
            }
            return null;
        }
        public bool ChangeStatus(int id)
        {
            return dao.ChangeStatus(id);
        }
        public Tuple<int, List<UnionModel>> GetUnionBookByCondition(int start, int lenght, int classId, string unionId, int status, int faculty, int semester)
        {
            var result = dao.GetUnionBookByCondition(start, lenght, classId, unionId, status, faculty, semester);
            var listUnion = new List<UnionModel>();
            foreach (var item in result.List)
            {
                UnionModel union = new UnionModel();
                union.id = item.ID;
                union.unionID = item.User1.StudentCode.Substring(0, 2) + "-" + item.NumID.ToString().PadLeft(5, '0');
                union.fullname = item.User1.FullName;
                union.studentCode = item.User1.StudentCode;
                union.className = item.User1.Class.Name;
                union.facultyName = item.User1.Class.Faculty.Name;
                union.create_At = item.Create_At;
                union.update_At = item.Update_At;
                union.create_at = String.Format("{0:dd/MM/yyyy h:mm tt}", item.Create_At);
                union.status = (int)item.Status;
                listUnion.Add(union);
            }
            return new Tuple<int, List<UnionModel>>(result.TotalRecords, listUnion);
        }
        public List<UnionModel> GetUnionBookByCondition(int classId, string unionId, int status, int faculty, int semester)
        {
            var result = dao.GetUnionBookByCondition(classId, unionId, status, faculty, semester);
            var listUnion = new List<UnionModel>();
            foreach (var item in result)
            {
                UnionModel union = new UnionModel();
                union.id = item.ID;
                union.unionID = item.User1.StudentCode.Substring(0, 2) + "-" + item.NumID.ToString().PadLeft(5, '0');
                union.fullname = item.User1.FullName;
                union.studentCode = item.User1.StudentCode;
                union.className = item.User1.Class.Name;
                union.facultyName = item.User1.Class.Faculty.Name;
                union.create_At = item.Create_At;
                union.update_At = item.Update_At;
                union.create_at = String.Format("{0:dd/MM/yyyy h:mm tt}", item.Create_At);
                union.status = (int)item.Status;
                listUnion.Add(union);
            }
            return listUnion;
        }
        public void SendReturnEmail(int unionID)
        {
            var sendMail = new EmailService();
            var user = dao.getUnionBookById(unionID);
            sendMail.Send(user.User1.Email, "<div style='width: 750px; padding: 10px ;margin: 0 auto; font-size: 1.2rem;'> <div style='border: 3px solid #222; padding: 1px; background-color: #fb0000cf;'> <div style='border: 3px solid #222; padding: 20px;background: #fdf6ec;'> <div style='text-align: center;'> <h1 style='font-size: 1.5rem; margin-top: 0; margin-bottom: 5px; text-transform: uppercase;'>Đoàn trường đại học công nghệ tp.hcm</h1> <span>-----o0o-----</span> <h3 style='font-size: 1.6rem; margin-top: 9px; text-transform: uppercase;'>biên nhận hồ sơ đoàn viên</h3> </div><div style='text-align: end;'> <p style='margin: 0;'>Sổ " + user.NumID.ToString().PadLeft(5,'0') + "/" + DateTime.Now.Year + "</p></div><div style='display:flex;'> <p style='margin: 9px 0;'>Họ và tên:</p><p style='margin: 9px 0;'>&emsp;" + user.User1.FullName + "</p></div><div style='display:flex;'> <div style='display:flex; width:50%'> <p style='margin: 9px 0;'>Ngày sinh:</p><p style='margin: 9px 0;'>&emsp;" + String.Format("{0:dd/MM/yyyy}", user.User1.Birthday) + "</p></div><div style='display:flex; width:50%'> <p style='margin: 9px 0;'>MSSV:</p><p style='margin: 9px 0;'>&emsp;" + user.User1.StudentCode + "</p></div></div><div style='display:flex;'> <div style='display:flex; width:50%'> <p style='margin: 9px 0;'>Lớp:</p><p style='margin: 9px 0;'>&emsp;" + user.User1.Class.Name + "</p></div><div style='display:flex; width:50%'> <p style='margin: 9px 0;'>Khoa:</p><p style='margin: 9px 0;'>&emsp;" + user.User1.Class.Faculty.Name + "</p></div></div><div style='margin-top: 20px;'> <p style='margin: 7px 0;'><b style='text-decoration:underline;'>Lưu ý:</b> Giữ gìn biên nhận cẩn thận, mang theo biên nhận khi rút sổ.</p><p style='margin: 7px 0;'>Vui lòng rút sổ trước ngày:" + String.Format("{0:dd/MM/yyyy}", user.ReturnDate) + "</p><p style='margin: 7px 0;'>Thắc mắc xin liên hệ <b>(028)-3512 0293</b> hoặc <b>doanthanhnien@hutech.edu.vn</b> </p></div><div style='text-align: end;'> <p>TP.Hồ Chí Minh, ngày ... tháng ... năm 20..</p><div style='width: 20%; margin-left: auto; margin-right: 80px; text-align: center;'> <p style='margin: 5px 0; font-weight: 600;'>Người nhận</p><p style='margin: 14px 0;'></p><p style='margin: 14px 0;'></p></div></div></div></div></div>");
        }

        /// <summary>
        /// Parse UnionModel from post data to entity model.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static UnionBook ParseModel(UnionModel model)
        {
            var book = new UnionBook();

            book.Create_At = DateTime.Now;
            book.Create_By = model.create_by;
            book.isEmail = model.isEmail;
            book.ReturnDate = model.returnDate;
            if (model.unionID != null)
            {
                book.NumID = Int32.Parse(model.unionID);
            }
            book.UserID = model.userID;
            book.Status = model.status;

            return book;
        }

        /// <summary>
        /// Insert a list union model. For ex, import a huge data from excel files.
        /// </summary>
        /// <param name="listData"></param>
        /// <returns></returns>
        public static List<int> SaveListUnionBook(List<UnionModel> listData)
        {
            // Convert list post data model to entity model
            var listEntityModel = new List<UnionBook>();
            
            foreach (var item in listData)
            {
                listEntityModel.Add(ParseModel(item));
            }

            // Save list model to table UnionBook
            return UnionDAO.InsertList(listEntityModel).Select(x => x.NumID.Value).ToList();
        }
        public int Update(UnionModel book)
        {
            var item = new UnionBook();
            item.ID = book.id;
            item.NumID = Convert.ToInt32(book.unionID);
            return dao.Update(item);
        }
    }
}