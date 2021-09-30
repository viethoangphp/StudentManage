using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StudentManage.Models
{
    public class PesonalEvalationModel : EvaluationModel
    {
        public int Score { set; get; }
        public string Proof { set; get; }
        public string Image { set; get; }
        public string Note { set; get; }
        
    }
}