using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StudentManage.Models
{
    public class PersonalScore
    {
        public int FormId { set; get; }
        public string FullName { set; get; }
        public string Semester { set; get;}
        public string Year { set; get; }
        public int Score { set; get; }
        public string Rank { set; get; }
        public string Status { set; get;}
        public string Note { set; get;}
    }
}