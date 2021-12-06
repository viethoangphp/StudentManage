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

        // Added Fields=================
        // Đoàn viên
        public int? score1 {  get; set; }
        // Bí thư chi đoàn
        public int? score2 { get; set; }
        // Bí thư đoàn khoa
        public int? score3 { get; set; }
        // Bí thư đoàn trường 
        public int? score4 {  get; set; }
    }
}