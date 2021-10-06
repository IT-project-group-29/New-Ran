namespace WebApplication4.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Program")]
    public partial class Program
    {
        [Key]
        [StringLength(30)]
        public string programCode { get; set; }

        [StringLength(100)]
        public string programDescription { get; set; }

        [StringLength(100)]
        public string programDirectorName { get; set; }
    }
}
