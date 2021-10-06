namespace WebApplication4.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ProjectPeopleAllocations
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int projectID { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int personID { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(15)]
        public string personRole { get; set; }

        public DateTime dateCreated { get; set; }

        public int creatorID { get; set; }

        [StringLength(500)]
        public string creatorComment { get; set; }

        public virtual Projects Projects { get; set; }
        [NotMapped]
        public virtual AspNetUsers Users { get; set; }
        
    }
}
