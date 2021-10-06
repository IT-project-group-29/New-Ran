namespace WebApplication4.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class PlanCourses
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int planId { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(10)]
        public string courseId { get; set; }

        [StringLength(10)]
        public string temp { get; set; }

        public virtual Course Course { get; set; }

        public virtual Plans Plans { get; set; }
    }
}
