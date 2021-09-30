using Models.EntityModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.BaseModel
{
    public class SemesterFormModel
    {
        public Semester semester { set; get; }
        public List<EvalutionForm> listForms { set; get; }
    }
}
