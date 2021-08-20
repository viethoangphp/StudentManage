using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using StudentManage.Models;
using Models.DAO;
using Models.EntityModel;
using System.Text.RegularExpressions;

namespace StudentManage.BUS
{
    public class FacultyBUS
    {
        public List<FacultyModel> GetListFaculty()
        {
            var result = new FacultyDAO().GetListFaculty();
            List<FacultyModel> list = new List<FacultyModel>();
            foreach (var item in result)
            {
                FacultyModel model = new FacultyModel();
                model.facultyID = item.FacutyID;
                model.facultyName = item.Name;
                list.Add(model);
            }
            return list;

        }
        public FacultyModel GetFacultyByName(string name)
        {
            var result = new FacultyDAO().GetFacultyByName(name);
            if (result != null)
            {
                FacultyModel model = new FacultyModel() { facultyID = result.FacutyID, facultyName = result.Name };
                return model;
            }
            return null;
        }
        public int InsertClass(ClassModel model)
        {
            Class classItem = new Class()
            {
                Name = model.className,
                FacutyID = model.facultyID,
                Status = model.status
            };
            return new FacultyDAO().InsertClass(classItem);
        }
        public ClassModel GetClassModelByName(string name)
        {
            var result = new FacultyDAO().GetClassByClassName(name);
            if (result != null)
            {
                return new ClassModel()
                {
                    classID = result.ClassID,
                    className = result.Name,
                    facultyID = result.FacutyID,
                    status = (int)result.Status
                };
            }
            return null;

        }
        public string ConvertFacultyName(string facultyName)
        {
            if(facultyName.Equals("Dược", StringComparison.OrdinalIgnoreCase))
            {
                return "Dược";
            }    
            if(facultyName.Equals("Luật", StringComparison.OrdinalIgnoreCase))
            {
                return "Luật";
            }
            facultyName = Regex.Replace(facultyName, @"[^0-9a-zA-ZĐƯĂÂÊ]+", "");
            switch (facultyName)
            {
                case "CNTT":
                    return "Công Nghệ Thông Tin";
                case "QTKD":
                    return "Quản Trị Kinh Doanh";
                case "HTTTQL":
                    return "Hệ Thống Thông Tin Quản Lý";
                case "KTMT":
                    return "Kiến Trúc Mỹ Thuật";
                case "NBH":
                    return "Nhật Bản Học";
                case "QTDLNHKS":
                    return "Quản Trị Du Lịch Nhà Hàng Khách Sạn";
                case "TA":
                    return "Tiếng Anh";
                case "TCTM":
                    return "Tài Chính Thương Mại";
                case "TTTK":
                    return "Truyền Thông Thiết Kế";
                case "VĐTNN":
                    return "Viện Đào Tạo Nghề Nghiệp";
                case "VĐTQT":
                    return "Viện Đào Tạo Quốc Tế";
                case "VKT":
                    return "Viện Kỹ Thuật";
                case "VJIT":
                    return "Viện Công Nghệ Việt Nhật";
                case "VKHUD":
                    return "Viện Khoa Học Ứng Dụng";
                case "VKHXHNV":
                    return "Viện Khoa Học Xã Hội Và Nhân Văn";
                case "VKIT":
                    return "Viện Công Nghệ Việt Hàn";
                case "XD":
                    return "Xây Dựng";
                default: return "";

            }
        }
        public string Convert(string fac)
        {
            string result = "";
            foreach (var ch in fac)
            {
                if (char.IsLetter(ch))
                {
                    result += ch;
                }
            }
            return result;
        }
        public int InsertFaculty(FacultyModel model)
        {
            Faculty faculty = new Faculty()
            {
                Name = model.facultyName,
                Phone = model.phone,
                Status = 1
            };
            return new FacultyDAO().InsertFaculty(faculty);
        }
    }
}