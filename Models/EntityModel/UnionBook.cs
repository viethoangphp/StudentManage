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
        public int ID { get; set; }

        public int? NumID { get; set; }

        public DateTime? Create_At { get; set; }

        public int? Create_By { get; set; }

        public int? UserID { get; set; }

        public DateTime? ReturnDate { get; set; }

        public int? Status { get; set; }

        public DateTime? Update_At { get; set; }

        public virtual User User { get; set; }

        public virtual User User1 { get; set; }
    }
}
