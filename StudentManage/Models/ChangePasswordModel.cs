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
        [MinLength(6)]
        public string passwordNew { set; get; }
        [Required]
        [MinLength(6)]
        [Compare("passwordNew")]
        public string confirmPassword { set; get;}
    }
}