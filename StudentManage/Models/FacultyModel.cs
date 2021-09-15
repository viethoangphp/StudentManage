using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace StudentManage.Models
{
    public class FacultyModel
    {
        public int facultyID { set; get;}
        [Required]
        public string facultyName { set; get;}
        [Required]
        public string phone { get; set; }
        public int totalClass { get; set; }
        public int status { get; set; }
    }
}