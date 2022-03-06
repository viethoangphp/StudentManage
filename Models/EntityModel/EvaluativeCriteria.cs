namespace Models.EntityModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("EvaluativeCriteria")]
    public partial class EvaluativeCriteria
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public EvaluativeCriteria()
        {
            DetailEvalutions = new HashSet<DetailEvalution>();
        }

        [Key]
        public int CriteriaID { get; set; }

        [StringLength(255)]
        public string CriteriaContent { get; set; }

        [StringLength(255)]
        public string CriteriaRequirement { get; set; }

        public int? Score { get; set; }

        public int MainID { get; set; }

        public int? IsImageProof { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DetailEvalution> DetailEvalutions { get; set; }

        public virtual EvaluativeMain EvaluativeMain { get; set; }
    }
}
