namespace Models.EntityModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("EvaluativeMain")]
    public partial class EvaluativeMain
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public EvaluativeMain()
        {
            EvaluativeCriterias = new HashSet<EvaluativeCriteria>();
        }

        [Key]
        public int MainID { get; set; }

        [StringLength(255)]
        public string Content { get; set; }

        public int? Status { get; set; }

        public int TemplateID { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<EvaluativeCriteria> EvaluativeCriterias { get; set; }

        public virtual TemplateForm TemplateForm { get; set; }
    }
}
