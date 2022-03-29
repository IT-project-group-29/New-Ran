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
        //public virtual AspNetUsers AspNetUsers { get; set; }
>>>>>>> b38892805380562e0a77e6f4729f5c6c48670588
    }

}
