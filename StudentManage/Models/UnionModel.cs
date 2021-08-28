using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StudentManage.Models
{
    public class UnionModel
    {
        public int id { set; get;}
        public string unionID { set; get; }
        public string fullname { set; get; }
        public string studentCode { set; get; }
        public string email { set; get; }
        public string phone { set; get; }
        public int gender { set; get; }
        public int cityId { set; get; }
        public int districtId { set; get; }
        public int wardId { set; get;}
        public string className { set; get; }
        public string birthDay { set; get; }
        public string facultyName { set; get; }
        public int facultyId { set; get;}
        public DateTime? joinDate { set; get; }
        public string JoinDate { set; get; }
        public string address { set; get;}
        public string create_at { set; get;}
        public DateTime? create_At { set; get; }
        public DateTime? update_At { set; get; }
        public DateTime? returnDate { set; get;}
        public string ReturnDate { set; get; }
        public int create_by { set; get; }
        public int userID { set; get;}
        public int status { set; get;}
        public int isEmail { set; get;}
    }
}