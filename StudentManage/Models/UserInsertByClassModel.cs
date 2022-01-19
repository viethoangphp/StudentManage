using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace StudentManage.Models
{
    public class UserInsertByClassModel
    {
        public int userID { set; get; }
        public int groupID { set; get; }
        public int positionID { set; get; }
        public int classID { set; get; }
        [Required]
        public string fullname { set; get; }
        [Required]
        public string email { set; get; }
        [Required]
        public string phone { set; get; }
        public string password { set; get; }
        public int gender { set; get; }

        public String birthDay { set; get; }
        [Required]
        public string joinDate { set; get; }
        public string avatar { set; get; }
        public int templateId { set; get; }

        [Required]
        public string className { set; get; }
        public int cityID { set; get; }
        public int districtID { set; get; }

        public string studentCode { set; get; }
        public string address { set; get; }
    }
}