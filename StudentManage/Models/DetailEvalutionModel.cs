using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StudentManage.Models
{
    public class DetailEvalutionModel
    {
        public int formId{ get; set; }
        public int userId{ get; set; }
        public int critetiaId{ get; set; }
        public int? score{ get; set; }
        public string note {  get; set; }
        public int? level {  get; set; }
        public string imageProof {  get; set; }

        public EvalutionFormModel evalutionForm = new EvalutionFormModel();
    }
}