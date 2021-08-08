namespace Models.EntityModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("GroupUser")]
    public partial class GroupUser
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public GroupUser()
        {
            Users = new HashSet<User>();
        }

        [Key]
        public int GroupId { get; set; }

        [StringLength(255)]
        public string Name { get; set; }

        public int? Status { get; set; }

        public int TimeID { get; set; }

        public int TemplateID { get; set; }

        public virtual TemplateForm TemplateForm { get; set; }

        public virtual TimeEvalution TimeEvalution { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<User> Users { get; set; }
    }
}
