using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using StudentManage.Models;
using Models.DAO;
using Models.EntityModel;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Security.Cryptography;
using StudentManage.Library;

namespace StudentManage.BUS
{
    public class UserBUS
    {
        protected UserDAO dao = new UserDAO();
        public UserModel GetUserByID(int id)
        {
            var model = dao.GetUserByID(id);
            if (model != null)
            {
                var user = new UserModel();
                user.userID = model.UserID;
                user.fullname = model.FullName;
                user.positionID = model.PositionID;
                user.email = model.Email;
                user.phone = model.Phone;
                user.gender = (model.Gender != null) ? (int)model.Gender: 1;
                user.studentCode = model.StudentCode.Trim();
                user.facultyName = (model.Class.Faculty.Name != null) ? model.Class.Faculty.Name : "";
                user.className = model.Class.Name;
                user.address = model.Address;
                user.birthDay = model.Birthday;
                user.cityID =(model.CityID != null)? (int)model.CityID:0;
                user.districtID = (model.DistrictID != null) ? (int)model.DistrictID:0;
                user.wardID = (model.WardID != null) ? (int)model.WardID:0;
                user.templateId = model.GroupUser.TemplateID;
                return user;
            }
            return null;
        }
        public int Insert(UserModel model)
        {
            var user = new User();
            user.GroupId = model.groupID;
            user.PositionID = model.positionID;
            user.ClassID = model.classID;
            user.FullName = toCapitalize(model.fullname);
            user.StudentCode = model.studentCode;
            user.Email = model.email;
            user.Phone = model.phone;
            user.Address = model.address;
            user.Birthday = model.birthDay;
            user.JoinDate = DateTime.ParseExact(model.joinDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            user.Date_End = model.date_end;
            user.Date_Start = model.date_start;
            user.Password = HashPassword.HashSHA256(model.password, new SHA256CryptoServiceProvider());
            user.Status = 1;
            user.CityID = model.cityID;
            user.DistrictID = model.districtID;
            user.WardID = model.wardID;
            user.Gender = model.gender;
            user.Avatar = model.avatar;
            return dao.Insert(user);
        }
        public bool ChangePassword(int userID ,string password,string passwordNew)
        {
            return dao.ChangePassword(userID, password,passwordNew);
        }
        public bool UpdateProfile(UserModel model)
        {
            var user = dao.GetUserByID(model.userID);
            if(user != null)
            {
                user.Birthday = model.birthDay;
                user.Gender = model.gender;
                user.Phone = model.phone;
                user.Email = model.email;
                user.Address = model.address;
                user.CityID = model.cityID;
                user.DistrictID = model.districtID;
                user.WardID = model.wardID;
                return dao.UpdateProfile(user);
            }
            return false;
            
        }

        public int InsertFromExcel(UserExcelModel model)
        {
            var user = new User();
            user.GroupId = 2;
            user.PositionID = 2;
            user.Gender = 1;
            var classID = dao.GetClassIDByClassName(model.className);
            // nếu không có thì thêm
            if (classID != -1)
            {
                user.ClassID = classID;
                user.FullName = toCapitalize(model.fullname);
                user.StudentCode = model.studentCode;
                user.Email = model.email;
                user.Phone = model.phone;
                user.Password = HashPassword.HashSHA256(model.studentCode, new SHA256CryptoServiceProvider());
                user.Status = 1;
                return dao.Insert(user);
            }
            else
            {
                // tạo class mới
                ClassModel classModel = new ClassModel();
                classModel.className = model.className.ToUpper();
                var facultyBUS = new FacultyBUS();
                var facultyName = facultyBUS.ConvertFacultyName(model.facultyName);
                if(facultyName != "")
                {
                    var faculty = facultyBUS.GetFacultyByName(facultyName);
                    if (faculty != null)
                    {
                        classModel.facultyID = faculty.facultyID;
                        classModel.status = 1;
                        var classId = facultyBUS.InsertClass(classModel);
                        user.ClassID = classId;
                        user.FullName = toCapitalize(model.fullname);
                        user.StudentCode = model.studentCode;
                        user.Email = model.email;
                        user.Phone = model.phone;
                        user.Password = HashPassword.HashSHA256(model.studentCode, new SHA256CryptoServiceProvider());
                        user.Status = 1;
                        return dao.Insert(user);
                    }
                }    
               
            }
            return -1;
            
        }
        public List<ClassModel> GetListClassByCondition(int facultyId,int year)
        {
            var result = dao.GetListClassByCondition(facultyId, year);
            var listClass = new List<ClassModel>();
            foreach(var item in result)
            {
                ClassModel model = new ClassModel();
                model.classID = item.ClassID;
                model.className = item.Name;
                listClass.Add(model);
            }
            return listClass;
        }
        public static string toCapitalize(string fullName)
        {
            fullName = Regex.Replace(fullName, "\\s+", " ").Trim();
            return  Regex.Replace(fullName.ToLower(), @"(^\w)|(\s\w)", m => m.Value.ToUpper());
        }
        public int Update(UserModel user)
        {
            var item = new User();
            item.UserID = user.userID;
            item.FullName = user.fullname;
            item.StudentCode = user.studentCode;
            item.Phone = user.phone;
            item.Email = user.email;
            item.Gender = user.gender;
            item.ClassID = user.classID;
            item.Birthday = user.birthDay;
            item.JoinDate = DateTime.ParseExact(user.joinDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            item.CityID = user.cityID;
            item.DistrictID = user.districtID;
            item.WardID = user.wardID;
            item.Address = user.address;
            item.Status = 1;
            return UserDAO.Update(item);
        }
        public List<UserModel> GetListUserByClass(int id)
        {
            List<User> listUser = new UserDAO().GetListUser().Where(m=>m.ClassID == id).ToList();
            List<UserModel> list = new List<UserModel>();
            foreach (var item in listUser)
            {
                UserModel model = new UserModel()
                {
                    className = item.Class.Name,
                    facultyName = item.Class.Faculty.Name,
                    fullname = item.FullName,
                    studentCode = item.StudentCode,
                    email = item.Email,
                    phone = item.Phone,
                    address = item.Address,
                    birthDay = item.Birthday,
                    cityID = item.CityID != null ? (int)item.CityID : 0,
                    districtID = item.DistrictID != null ? (int)item.DistrictID : 0,
                    wardID = item.WardID != null ? (int)item.WardID : 0,
                    gender = item.Gender != null ? (int)item.Gender : 1
                };
                list.Add(model);
            }
            return list;
        }
    }
}