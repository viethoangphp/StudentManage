using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StudentManage.Models
{
    public class SchoolEvaluationModel
    {
        public int FacultyId { get; set; }
        public string FacultyName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public int? FacultyDone { get; set; }
        public int? FacultyNotDone { get; set; }
        public int? SchoolDone { get; set; }
        public int? SchoolNotDone { get; set; }
        public int? FacultyCondition { set; get; }
        public int? SchoolCondition { set; get; }
    }
}