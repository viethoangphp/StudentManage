namespace Models.EntityModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("User")]
    public partial class User
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public User()
        {
            DetailEvalutions = new HashSet<DetailEvalution>();
            Tickets = new HashSet<Ticket>();
            Tickets1 = new HashSet<Ticket>();
            UnionBooks = new HashSet<UnionBook>();
            UnionBooks1 = new HashSet<UnionBook>();
        }

        public int UserID { get; set; }

        public int GroupId { get; set; }

        public int PositionID { get; set; }

        public int? ClassID { get; set; }

        [StringLength(255)]
        public string FullName { get; set; }

        [StringLength(10)]
        public string StudentCode { get; set; }

        [StringLength(50)]
        public string Email { get; set; }

        [StringLength(11)]
        public string Phone { get; set; }

        [StringLength(255)]
        public string Address { get; set; }

        public DateTime? Birthday { get; set; }

        public DateTime? Date_Start { get; set; }

        public DateTime? Date_End { get; set; }

        [StringLength(256)]
        public string Password { get; set; }

        [StringLength(255)]
        public string Note { get; set; }

        [StringLength(1)]
        public string Type { get; set; }

        public int? Status { get; set; }

        public int? CityID { get; set; }

        public int? DistrictID { get; set; }

        public int? WardID { get; set; }

        public int? Gender { get; set; }

        [StringLength(50)]
        public string Avatar { get; set; }

        public DateTime? JoinDate { get; set; }

        public virtual Class Class { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DetailEvalution> DetailEvalutions { get; set; }

        public virtual GroupUser GroupUser { get; set; }

        public virtual Position Position { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Ticket> Tickets { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Ticket> Tickets1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UnionBook> UnionBooks { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UnionBook> UnionBooks1 { get; set; }
    }
}
