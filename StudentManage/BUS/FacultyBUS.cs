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
                model.phone = item.Phone != null ? item.Phone.Trim() : null;
                model.status = (int)item.Status;
                model.totalClass = GetListClassByFaculty(item.FacutyID).Count();
                list.Add(model);
            }
            return list;

        }
        public FacultyModel GetFacultyByName(string name)
        {
            var result = new FacultyDAO().GetFacultyByName(name);
            if (result != null)
            {
                FacultyModel model = new FacultyModel() 
                { 
                    facultyID = result.FacutyID, 
                    facultyName = result.Name 
                };
                return model;
            }
            return null;
        }
        public Tuple<int, List<ClassModel>> GetListClass(int start,int length)
        {
            var result = new FacultyDAO().GetListClass(start,length);
            var listUser = new UserDAO().GetListUser(); 
            List<ClassModel> list = new List<ClassModel>();
            foreach (var item in result.List)
            {
                ClassModel model = new ClassModel()
                {
                    classID = item.ClassID,
                    className = item.Name,
                    facultyID = item.FacutyID,
                    facultyName = item.Faculty.Name,
                    totalStudent = listUser.Where(m=>m.Class.Name.Contains(item.Name)).Count(), //Try optimizing performance this line. Original code: totalStudent = new UserDAO().GetListUserByClassName(item.Name).Count()
                    status = (int)item.Status
                };
                list.Add(model);
            }
            return new Tuple<int,List<ClassModel>>(result.TotalRecords,list);
        }
        public List<ClassModel> GetListClassByFaculty(int id)
        {
            List<ClassModel> list = new List<ClassModel>();
            var result = new FacultyDAO().GetListClassByFaculty(id);
            foreach (var item in result)
            {
                ClassModel model = new ClassModel()
                {
                    classID = item.ClassID,
                    className = item.Name,
                    status = (int)item.Status
                };
                list.Add(model);
            }
            return list;
        }
        public int InsertClass(ClassModel model)
        {
            Class classItem = new Class()
            {
                Name = model.className,
                FacutyID = model.facultyID,
                Status = 1
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
        public FacultyModel GetFacultyByID(int id)
        {
            Faculty fac = new FacultyDAO().GetFacultyByID(id);
            if(fac != null)
            {
                FacultyModel model = new FacultyModel()
                {
                    facultyID = fac.FacutyID,
                    facultyName = fac.Name,
                    phone = fac.Phone != null ? fac.Phone.Trim() : null,
                };
                return model;
            }
            return null;
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
        public int UpdateFaculty(FacultyModel model)
        {
            Faculty faculty = new FacultyDAO().GetFacultyByID(model.facultyID);
            if (faculty != null)
            {
                faculty.Name = model.facultyName;
                faculty.Phone = model.phone;
                return new FacultyDAO().UpdateFaculty(faculty);
            }
            else
            {
                return 0;
            }
        }
        public int DeleteFaculty(int id)
        {
            return new FacultyDAO().DeleteFaculty(id);
        }
        public ClassModel GetClassByID(int id)
        {
            Class cls = new FacultyDAO().GetClassByID(id);
            if(cls != null)
            {
                ClassModel model = new ClassModel()
                {
                    classID = cls.ClassID,
                    className = cls.Name,
                    facultyID = cls.FacutyID,
                    facultyName = cls.Faculty.Name
                };
                return model;
            }
            return null;
        }
        public int UpdateClass(ClassModel model)
        {
            Class cls = new FacultyDAO().GetClassByID(model.classID);
            if (cls != null)
            {
                cls.ClassID = model.classID;
                cls.Name = model.className;
                cls.FacutyID = model.facultyID;
                return new FacultyDAO().UpdateClass(cls);
            }
            else
            {
                return 0;
            }
        }
        public int DeleteClass(int id)
        {
            return new FacultyDAO().DeleteClass(id);
        }
        public Tuple<int, List<ClassModel>> GetListClassByCondition(int start, int lenght, int facultyID, string className)
        {
            var result = new FacultyDAO().GetListClassByCondition(start, lenght, facultyID, className);
            var listCls = new List<ClassModel>();
            var listUser = new UserDAO().GetListUser();
            foreach (var cls in result.List)
            {
                ClassModel model = new ClassModel()
                {
                    classID = cls.ClassID,
                    className = cls.Name,
                    facultyID = cls.FacutyID,
                    facultyName = cls.Faculty.Name,
                    totalStudent = listUser.Where(m => m.Class.Name.Contains(cls.Name)).Count(),
                };
                listCls.Add(model);
            }
            return new Tuple<int, List<ClassModel>>(result.TotalRecords, listCls);
        }
    }
}