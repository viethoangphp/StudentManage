using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StudentManage.Models
{
    public class EvaluationModel
    {
        //QuangMinh
        public int criteriaId { get; set; }
        public int? score {  get; set; }
        public string imageProof {  get; set; }
        //Hoang
        public int CriteriaID { set; get; }
        public string Content { set; get; }
        public string Requirement { set; get;}
        public int MaxScore { set; get;}
      
        public string note {  get; set; }

        public int? Order { get; set; }
    }
}