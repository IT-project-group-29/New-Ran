namespace WebApplication4.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ProjectEffortsApplied")]
    public partial class ProjectEffortsApplied
    {
        public int projectEffortsAppliedID { get; set; }

        public int projectID { get; set; }

        public int effortID { get; set; }

        [StringLength(100)]
        public string comment { get; set; }

        public int? hours { get; set; }

        public virtual ProjectEfforts ProjectEfforts { get; set; }

        public virtual Projects Projects { get; set; }
    }
}
