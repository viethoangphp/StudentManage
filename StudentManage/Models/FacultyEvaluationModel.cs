using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StudentManage.Models
{
    public class FacultyEvaluationModel
    {
        public string ClassName { get; set; }
        public int? Total { get; set; }
        public int? ClassDone { get; set; }
        public int? ClassNotDone { get; set; }
        public int? FacultyDone { get; set; }
        public int? FacultyNotDone { get; set; }
        public int ClassId { get; set; }

        public int? ClassCondition {get; set; }
        public int? FacultyCondition { get; set; }
        
    }
}