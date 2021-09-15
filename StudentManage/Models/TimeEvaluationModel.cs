using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StudentManage.Models
{
    public class TimeEvaluationModel
    {
        public int timeId {  get; set; }
        public DateTime dateStart {  get; set; }
        public DateTime dateEnd {  get; set; }
        public int status {  get; set; }
    }
}