namespace WebApplication4.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Clients
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int clientID { get; set; }

        [StringLength(100)]
        public string companyName { get; set; }
    }
}
