using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StudentManage.Models
{
    public class ClassModel
    {
        public int classID { set; get;}
        public string className { set; get;}
        public int facultyID { set; get; }
        public int status { set; get; }

    }
}