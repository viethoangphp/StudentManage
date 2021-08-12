using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using StudentManage.Models;
using Models.DAO;
using Models.EntityModel;

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
                user.email = model.Email;
                user.phone = model.Phone;
                user.gender = (int)model.Gender;
                user.studentCode = model.StudentCode;
                user.facultyName = model.Class.Faculty.Name;
                user.className = model.Class.Name;
                user.address = model.Address;
                user.birthDay = model.Birthday;
                user.cityID =(int)model.CityID;
                user.districtID = (int)model.DistrictID;
                user.wardID = (int)model.WardID;

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
            user.FullName = model.fullname;
            user.StudentCode = model.studentCode;
            user.Email = model.email;
            user.Phone = model.phone;
            user.Address = model.address;
            user.Birthday = model.birthDay;
            user.Date_End = model.date_end;
            user.Date_Start = model.date_start;
            user.Password = model.password;
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
            var classID = dao.GetClassIDByClassName(model.className);
            if (classID != -1)
            {
                user.ClassID = classID;
                user.FullName = model.fullname;
                user.StudentCode = model.studentCode;
                user.Email = model.email;
                user.Phone = model.phone;
                user.Password = model.studentCode;
                user.Status = 1;
                return dao.Insert(user);
            }
            return -1;
            
          
           
        }
    }
}