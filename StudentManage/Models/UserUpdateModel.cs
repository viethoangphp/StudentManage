using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace StudentManage.Models
{
    public class UserUpdateModel
    {
        public int userID { set; get; }
        public int groupID { set; get; }
        public int positionID { set; get; }
        [Required]
        public string fullname { set; get; }
        [Required]
        public string email { set; get; }
        public string phone { set; get; }
        public string password { set; get; }
        public int gender { set; get; }
        [Required]
        public string birthday { set; get; }
        [Required]
        public string joinDate { set; get; }
        public string address { set; get; }
        public int cityID { set; get; }
        public int districtID { set; get; }
        public int wardID { set; get; }
    }
}