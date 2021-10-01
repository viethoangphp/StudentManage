using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StudentManage.Models
{
    public class FormModel
    {
        public int FormId { set; get; }
        public int Total { set; get; }
        public DateTime Create_At { set; get; }
        public int Create_By { set; get; }
        public int Status { set; get; }
        public int SemesterId { set; get; }
    }
}