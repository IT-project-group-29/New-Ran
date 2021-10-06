namespace WebApplication4.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ProjectMethodsApplied")]
    public partial class ProjectMethodsApplied
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int projectID { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int methodID { get; set; }

        [StringLength(100)]
        public string comment { get; set; }

        public virtual ProjectMethods ProjectMethods { get; set; }

        public virtual Projects Projects { get; set; }
    }
}
