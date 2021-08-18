using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace StudentManage.Models
{
    public class ChangePasswordModel
    {
        [Required]
        public string passwordOld { set; get;}
        [Required]
        public string passwordNew { set; get; }
        [Required]
        public string confirmPassword { set; get;}
    }
}