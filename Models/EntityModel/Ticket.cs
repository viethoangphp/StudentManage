namespace Models.EntityModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Ticket")]
    public partial class Ticket
    {
        [Key]
        public int TickID { get; set; }

        public DateTime? Create_At { get; set; }

        [StringLength(255)]
        public string ContentTicket { get; set; }

        [StringLength(255)]
        public string Location { get; set; }

        public int? Status { get; set; }

        [StringLength(255)]
        public string Response { get; set; }

        public int? Response_by { get; set; }

        public int? Create_by { get; set; }

        public DateTime? Date_Return { get; set; }

        public virtual User User { get; set; }

        public virtual User User1 { get; set; }
    }
}
