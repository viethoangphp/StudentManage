using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StudentManage.Models
{
    public class PersonalFormModel
    {
        public string StudentCode {  get; set; }
        public string FullName {  get; set; }

        public int formId {  get; set; }
        public DateTime? BirthDate { get; set; }
        // Điểm
        public int? Score1 { get; set; }
        public int? Score2 { get; set; }
        public int? Score3 { get; set; }
        public int? Score4 { get; set; }
        // Xếp Loại
        public string Ranking {  get; set; }
        // Trạng thái
        public string Situation {  get; set; }
        // Ghi chú
        public string Note {  get; set; }

    }
}