using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StudentManage.Models
{
    public class DetailFormModel
    {
        public int FormId { set; get; }
        public int UserId { set; get; }
        public int CriteriaId { set; get;}
        public int Score { set; get;}
        public string Note { set; get; }
        public int Level { set; get; }
        public string Image { set; get; }
        public HttpPostedFileBase file { set; get; }
    }
}