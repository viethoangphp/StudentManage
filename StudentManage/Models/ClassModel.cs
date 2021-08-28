using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace StudentManage.Models
{
    public class ClassModel
    {
        public int classID { set; get;}
        [Required]
        public string className { set; get;}
        [Required]
        public int facultyID { set; get; }
        public string facultyName { get; set; }
        public int totalStudent { get; set; }
        public int status { set; get; }

    }
}