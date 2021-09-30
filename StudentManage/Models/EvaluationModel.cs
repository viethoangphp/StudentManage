using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StudentManage.Models
{
    public class EvaluationModel
    {
        public int CriteriaID { set; get; }
        public string Content { set; get; }
        public string Requirement { set; get;}
        public int MaxScore { set; get;}

    }
}