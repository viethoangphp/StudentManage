using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using Models.DAO;
using StudentManage.Library;
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
            password = HashPassword.HashSHA256(password, new SHA256CryptoServiceProvider());
            if (user != null)
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