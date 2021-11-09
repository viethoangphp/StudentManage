namespace Models.EntityModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("DetailEvalution")]
    public partial class DetailEvalution
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int FormId { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int UserID { get; set; }

        [Key]
        [Column(Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int CriteriaID { get; set; }

        [Key]
        [Column(Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Level { get; set; }

        public int? Score { get; set; }

        [StringLength(255)]
        public string Note { get; set; }


        [StringLength(50)]
        public string Image_proof { get; set; }

        public virtual EvaluativeCriteria EvaluativeCriteria { get; set; }

        public virtual EvalutionForm EvalutionForm { get; set; }

        public virtual User User { get; set; }
    }
}
