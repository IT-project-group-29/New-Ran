namespace WebApplication4.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class PeopleComments
    {
        [Key]
        public int commentID { get; set; }

        public int personID { get; set; }

        public DateTime commentDate { get; set; }

        public int commentPersonID { get; set; }

        [Required]
        [StringLength(500)]
        public string comment { get; set; }

        public bool flag { get; set; }
    }
}
