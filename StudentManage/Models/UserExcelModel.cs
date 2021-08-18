using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace StudentManage.Models
{
    public class UserExcelModel
    {
        [Required]
        public string fullname { set; get;}
        [Required]
        public string studentCode { set; get; }
        [Required]
        public string phone { set; get; }
        [Required]
        public string email { set; get; }
        [Required]
        public string className { set; get;}
        [Required]
        public int unionID { set; get; }
        [Required]
        public string facultyName { set; get; }
    }
}