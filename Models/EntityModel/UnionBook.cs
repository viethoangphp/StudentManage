namespace Models.EntityModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("UnionBook")]
    public partial class UnionBook
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [StringLength(6)]
        public string CharID { get; set; }

        public DateTime? Create_At { get; set; }

        public int? Create_By { get; set; }

        public int? UserID { get; set; }

        public int? Status { get; set; }

        public virtual User User { get; set; }

        public virtual User User1 { get; set; }
    }
}
