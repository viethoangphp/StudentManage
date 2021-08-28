namespace Models.EntityModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Config")]
    public partial class Config
    {
        [Key]
        [StringLength(50)]
        public string KeyName { get; set; }

        [StringLength(255)]
        public string Value { get; set; }
    }
}
