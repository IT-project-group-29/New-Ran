using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication4.Models
{
    public partial class Clients
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int clientID { get; set; }

        [StringLength(100)]
        public string companyName { get; set; }
<<<<<<< HEAD

=======
>>>>>>> ff2fd70001df947002414e94c94821042304620b
    }
}

