using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace StudentManage.Models
{
    public class UserExcelModel
    {
        
        public string fullname { set; get;}
       
        public string studentCode { set; get; }
       
        public string phone { set; get; }
      
        public string email { set; get; }
       
        public string className { set; get;}
        
        public string unionID { set; get; }
     
        public string facultyName { set; get; }
        
    }
}