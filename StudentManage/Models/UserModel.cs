using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StudentManage.Models
{
    public class UserModel
    {
        public int userID { set; get; }
        public string fullname { set; get; }
        public string email { set; get; }
        public string phone { set; get; }
        public int gender { set; get; }
        public DateTime? birthDay { set; get; }
        public string address { set; get; }

    }
}