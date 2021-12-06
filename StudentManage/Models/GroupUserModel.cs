using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DAO
{
    public class GroupUserModel
    {
        public int groupId {  get; set; }
        public string name { get; set; }
        public int status {  get; set; }
        public int timeId {  get; set; }
        public int templateId {  get; set; }
    }
}
