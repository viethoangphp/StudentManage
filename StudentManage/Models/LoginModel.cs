using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Models.DAO;
namespace StudentManage.Models
{
    public class LoginModel
    {
        [Required]
        public string username { set; get; }
        [Required]
        public string password { set; get; }
        public int CheckLogin(string username , string password)
        {
            var dao = new UserDAO();
            var user = dao.GetUserByUsername(username);
            if(user != null)
            {
                if(user.Password.Trim() == password)
                {
                    return user.UserID;
                }    
            }
            return -1;
        }
    }
}