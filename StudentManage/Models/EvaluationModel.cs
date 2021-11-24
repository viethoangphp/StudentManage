using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StudentManage.Models
{
    public class EvaluationModel
    {
        public int criteriaId { get; set; }
        public int? score {  get; set; }
        public string imageProof {  get; set; }
        public string note {  get; set; }

        public int? Order { get; set; }
    }
}