using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StudentManage.Models
{
    public class EvaluationMainModel
    {
        public int MainID { set; get;}
        public string Title { set; get; }
        public List<PesonalEvalationModel> ListRequriement { set; get;}
    }
}