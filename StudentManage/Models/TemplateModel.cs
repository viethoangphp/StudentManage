using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StudentManage.Models
{
    public class TemplateModel
    {
        public int TemplateID { set; get;}
        public int FormID   { set; get;}
        public string TemplateName { set; get;}
        public List<EvaluationMainModel> ListMain { set; get;}
        public string FullName { set; get; }
        public string Position { set; get; }
        public string Email { set; get; }
        public string Phone { set; get; }
        public string Faculty { set; get; }
        public int TotalScore { set; get; }
        public string Rank { set; get; }
       
    }
}