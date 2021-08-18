using Models.EntityModel;
using System.Collections.Generic;

namespace Models.BaseModel
{
    public class UnionBookByConditionModel
    {
        public List<UnionBook> List { get; set; }
        public int TotalRecords { get; set; }
    }
}
