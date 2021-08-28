using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StudentManage.Models
{
    public class EvaluativeCriteriaModel
    {
        public int criteriaId { get; set; }
        public string criteriaContent {  get; set; }
        public string criteriaRequirement { get; set; }
        public int? score {  get; set; }
        public int mainId {  get; set; }
    }
}