using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using StudentManage.Models;
using Models.DAO;
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
                user.address = model.Address;
                user.birthDay = model.Birthday;
                return user;
            }
            return null;
        }
    }
}