using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StudentManage.Models
{
    public class EvaluationModelM
    {
        public int criteriaId { get; set; }
        public int? score { get; set; }
        public HttpPostedFileBase imageProof { get; set; }
        public string ImageURL { get; set; }
        public string note { get; set; }

        public int? Order { get; set; }
    }
}