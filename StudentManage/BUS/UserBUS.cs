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
                user.groupID = model.GroupId;
                if (model.ClassID != null)
                {
                    user.classID = (int)model.ClassID;
                }
                user.positionID = model.PositionID;
                user.email = model.Email;
                user.phone = model.Phone;
                user.gender = (model.Gender != null) ? (int)model.Gender : 1;
                user.studentCode = model.StudentCode == null ? "" : model.StudentCode.Trim();
                user.facultyName = (model.Class.Faculty.Name != null) ? model.Class.Faculty.Name : "";
                user.className = model.Class.Name;
                user.address = model.Address;
                user.birthDay = model.Birthday;
                user.cityID = (model.CityID != null) ? (int)model.CityID : 0;
                user.districtID = (model.DistrictID != null) ? (int)model.DistrictID : 0;
                user.wardID = (model.WardID != null) ? (int)model.WardID : 0;
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
        public bool ChangePassword(int userID, string password, string passwordNew)
        {
            return dao.ChangePassword(userID, password, passwordNew);
        }
        public bool UpdateProfile(UserModel model)
        {
            var user = dao.GetUserByID(model.userID);
            if (user != null)
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
                if (facultyName != "")
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
        public List<ClassModel> GetListClassByCondition(int facultyId, int year)
        {
            var result = dao.GetListClassByCondition(facultyId, year);
            var listClass = new List<ClassModel>();
            foreach (var item in result)
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
            return Regex.Replace(fullName.ToLower(), @"(^\w)|(\s\w)", m => m.Value.ToUpper());
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
            return new UserDAO().Update(item);
        }
        public List<UserModel> GetListUserByClass(int id)
        {
            List<User> listUser = new UserDAO().GetListUser().Where(m => m.ClassID == id).ToList();
            List<UserModel> list = new List<UserModel>();
            foreach (var item in listUser)
            {
                UserModel model = new UserModel()
                {
                    userID = item.UserID,
                    className = item.Class.Name,
                    classID = (int)item.ClassID,
                    facultyName = item.Class.Faculty.Name,
                    fullname = item.FullName,
                    studentCode = item.StudentCode,
                    email = item.Email,
                    phone = item.Phone,
                    groupID = item.GroupId,
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

        #region Get list user
        //Get list user (for scoring)
        public List<UserModel> GetListUserByPage(int? pageNum)
        {
            List<User> listUser = new UserDAO().GetListUserByPage(pageNum);
            List<UserModel> list = new List<UserModel>();
            foreach (var item in listUser)
            {
                if (item.Status == 1)
                {
                    UserModel model = new UserModel()
                    {
                        userID = item.UserID,
                        className = item.Class.Name,
                        positionName = item.Position.Name,
                        facultyName = item.Class.Faculty.Name,
                        fullname = item.FullName,
                        studentCode = item.StudentCode,
                        email = item.Email,
                        phone = item.Phone,
                        address = item.Address,
                        birthDayString = item.Birthday != null ? ((DateTime)item.Birthday).ToString("dd/MM/yyyy") : null,
                        cityID = item.CityID != null ? (int)item.CityID : 0,
                        districtID = item.DistrictID != null ? (int)item.DistrictID : 0,
                        wardID = item.WardID != null ? (int)item.WardID : 0,
                        gender = item.Gender != null ? (int)item.Gender : 1
                    };
                    list.Add(model);
                }
            }
            return list;
        }
        //List position name
        public List<PositionModel> GetListPosition()
        {
            List<Position> listPosition = new UserDAO().GetListPosition();
            List<PositionModel> list = new List<PositionModel>();
            foreach (var item in listPosition)
            {
                PositionModel model = new PositionModel()
                {
                    PositionID = item.PositionID,
                    PositionName = item.Name
                };
                list.Add(model);
            }
            return list;
        }
        //List group name
        public List<GroupModel> GetListGroup()
        {
            List<GroupUser> listGroup = new UserDAO().GetListGroup();
            List<GroupModel> list = new List<GroupModel>();
            foreach (var item in listGroup)
            {
                GroupModel model = new GroupModel()
                {
                    GroupID = item.GroupId,
                    GroupName = item.Name
                };
                list.Add(model);
            }
            return list;
        }
        #endregion

        #region User Insert
        public int InsertUser(UserInsertModel model)
        {
            var user = new User
            {
                GroupId = 9 - model.positionID,
                PositionID = model.positionID,
                ClassID = model.classID,
                FullName = toCapitalize(model.fullname),
                Email = model.email,
                Phone = model.phone,
                Birthday = model.birthDay,
                JoinDate = DateTime.ParseExact(model.joinDate, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                Password = HashPassword.HashSHA256(model.password, new SHA256CryptoServiceProvider()),
                Status = 1,
                Gender = model.gender
            };
            return dao.Insert(user);
        }
        #endregion

        #region User Update
        public int UpdateUser(UserUpdateModel model)
        {
            var item = new User
            {
                UserID = model.userID,
                FullName = model.fullname,
                Phone = model.phone,
                Email = model.email,
                Gender = model.gender,
                //ClassID = model.classID,
                Birthday = DateTime.ParseExact(model.birthday, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                JoinDate = DateTime.ParseExact(model.joinDate, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                //CityID = model.cityID,
                //DistrictID = model.districtID,
                //WardID = model.wardID,
                Address = model.address,
                PositionID = model.positionID,
                Status = 1
            };
            return new UserDAO().UpdateUser(item);
        }
        //Get user info
        public UserUpdateModel GetUpdateUserInfo(int id)
        {
            User user = new UserDAO().GetUserByID(id);
            if (user == null) return null;
            UserUpdateModel model = new UserUpdateModel()
            {
                address = user.Address == null ? "" : user.Address,
                birthday = user.Birthday == null ? "" : ((DateTime)user.Birthday).ToString("dd/MM/yyyy"),
                cityID = user.CityID == null ? 0 : (int)user.CityID,
                districtID = user.DistrictID == null ? 0 : (int)user.DistrictID,
                email = user.Email == null ? "" : user.Email,
                fullname = user.FullName == null ? "" : user.FullName,
                gender = user.Gender == null ? 0 : (int)user.Gender,
                groupID = user.GroupId,
                joinDate = user.JoinDate == null ? "" : ((DateTime)user.JoinDate).ToString("dd/MM/yyyy"),
                phone = user.Phone == null ? "" : user.Phone.Trim(),
                positionID = user.PositionID,
                userID = user.UserID,
                wardID = user.WardID == null ? 0 : (int)user.WardID
            };
            return model;
        }
        #endregion

        #region User Delete
        public int DeleteUser(int id)
        {
            return new UserDAO().DeleteUser(id);
        }
        #endregion

        #region User Count
        public int GetUserCount()
        {
            return new UserDAO().GetUserCount();
        }
        #endregion
    }
}