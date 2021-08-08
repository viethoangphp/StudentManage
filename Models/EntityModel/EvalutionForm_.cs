namespace Models.EntityModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class EvalutionForm_
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public EvalutionForm_()
        {
            DetailEvalutions = new HashSet<DetailEvalution>();
        }

        [Key]
        public int FormId { get; set; }

        public DateTime? Create_At { get; set; }

        public int? Create_by { get; set; }

        public int? Total { get; set; }

        public int? Status { get; set; }

        public int SemesterID { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DetailEvalution> DetailEvalutions { get; set; }

        public virtual Semester Semester { get; set; }
    }
}
