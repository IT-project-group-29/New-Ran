namespace WebApplication4.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class PeopleContacts
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int personID { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(15)]
        public string contactMethod { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(100)]
        public string contactValue { get; set; }

        public bool preferred { get; set; }

        public DateTime dateCreated { get; set; }

        public DateTime? dateEnded { get; set; }
    }
}
