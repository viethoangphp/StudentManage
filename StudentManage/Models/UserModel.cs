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
        [Required]
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
        [Required]
        public string birthDayString { set; get;}
        [Required]
        public string joinDate { set; get; }
        public string address { set; get; }
        public string facultyName { set; get; }
        [Required]
        public int facultyID { set; get;}
        [Required]
        public string className { set; get;}
        public int cityID { set; get; }
        public int districtID { set; get; }
        public int wardID { set; get;}
        public string avatar { set; get;}
        public int templateId { set; get;}
    }
}