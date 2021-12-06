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
        public int? FacultyDone { get; set; }
        public int? FacultyNotDone { get; set; }
        public int? SchoolDone { get; set; }
        public int? SchoolNotDone { get; set; }
        public int? FacultySituation { set; get; }
        public int? SchoolSituation { set; get; }
        public int? Total { get; set; }
    }
}