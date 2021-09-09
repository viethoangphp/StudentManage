using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StudentManage.Models
{
    public class EvalutionFormModel
    {
        public int formId { get; set; }
        public DateTime? createAt { get; set; }
        public int? createBy { get; set; }
        public int? total {  get; set; }
        public int? status { get; set; }
        public int semesterId{ get; set; }

        //Added field
        public bool? inProcess { set; get; }

        

    }
}