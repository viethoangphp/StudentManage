using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StudentManage.Models
{
    public class SemesterModel
    {
        public int semesterId { set; get; }

        public string name { set; get; }

        public string year { set; get; }

        public DateTime? dayStart { set; get; }

        public DateTime? dayEnd { set; get; }

        public int? status { set; get; }

        // Added Fields
        public int? FormId { set; get; }
        public int? score1 { set; get; }
        public int? score2 { set; get; }
        public int? score3 { set; get; }
        public int? score4 { set; get; }
        public bool? inProcess { set; get; }
        public string Note {  set; get; }


    }
}