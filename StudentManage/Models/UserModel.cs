using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace StudentManage.Models
{
    public class UserModel
    {
        public int userID { set; get; }
        public int groupID { set; get; }
        public int positionID { set; get; }
        public int classID { set; get;}
        public string fullname { set; get; }
        [Required]
        public string email { set; get; }
        [Required]
        public string phone { set; get; }
        [Required]
        public string studentCode { set; get; }
        public DateTime? date_end { set; get;}
        public DateTime? date_start { set; get;}
        public string password { set; get;}
        public int gender { set; get; }
        public DateTime? birthDay { set; get; }
        public string birthDayString { set; get;}
        [Required]
        public string address { set; get; }
        public string facultyName { set; get; }
        public string className { set; get;}
        [Required]
        public int cityID { set; get; }
        [Required]
        public int districtID { set; get; }
        [Required]
        public int wardID { set; get;}
        public string avatar { set; get;}
    }
}