using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StudentManage.Models
{
    public class SearchUnionModel
    {
        public int semesterId { set; get;}
        public int facultyId { set; get; }
        public int classId { set; get; }
        public int statusId { set; get; }
        public int unionId { set; get; }
    }
}