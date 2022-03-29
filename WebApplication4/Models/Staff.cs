namespace WebApplication4.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Staff")]
    public partial class Staff
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int staffID { get; set; }

        [StringLength(12)]
        public string uniStaffID { get; set; }

        [StringLength(12)]
        public string username { get; set; }

        [Column(TypeName = "date")]
        [Required]
        public DateTime? dateEnded { get; set; }

        public int projnum { get; set; }

        public int diffnum { get; set; }

        [NotMapped]
        public virtual AspNetUsers AspNetUsers { get; set;}
    }
}