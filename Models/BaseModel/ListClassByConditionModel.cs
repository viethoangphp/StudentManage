using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.EntityModel;

namespace Models.BaseModel
{
    public class ListClassByConditionModel
    {
        public List<Class> List { get; set; }
        public int TotalRecords { get; set; }
    }
}
