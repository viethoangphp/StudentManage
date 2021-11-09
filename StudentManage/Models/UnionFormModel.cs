﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StudentManage.Models
{
    public class UnionFormModel
    {
        public List<EvaluativeMainModel> ListMain {  get; set; }
        public List<EvaluativeCriteriaModel> ListCriteria {  get; set; }
        public int[] Total {  get; set; }
        public int? Turn { get; set; }
        public int IsInTime {  get; set; }
        
    }
}