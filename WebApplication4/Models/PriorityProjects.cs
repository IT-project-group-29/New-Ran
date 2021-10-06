namespace WebApplication4.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class PriorityProjects
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int projectID { get; set; }

        [Key]
        [Column(Order = 1)]
        public DateTime dateCreated { get; set; }

        [Required]
        [StringLength(3)]
        public string priorityLevel { get; set; }

        [StringLength(500)]
        public string priorityReason { get; set; }

        public int? priorityCreatorID { get; set; }

        public virtual Projects Projects { get; set; }
    }
}
