using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StudentManage.Models
{
    public class EmailSettingsModel
    {
        public string EmailUser { get; set; }
        public string EmailPass { get; set; }
        public string EmailTitle { get; set; }
        public string EmailSubject { get; set; }
    }
}