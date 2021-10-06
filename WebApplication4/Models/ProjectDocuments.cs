namespace WebApplication4.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ProjectDocuments
    {
        [Key]
        public int projectDocumentID { get; set; }

        public int projectID { get; set; }

        [Required]
        [StringLength(10)]
        public string documentSource { get; set; } = "";

        [Required]
        [StringLength(250)]
        public string documentLink { get; set; } = "";

        [Required]
        [StringLength(250)]
        public string documentTitle { get; set; } = "";

        [StringLength(500)]
        public string filePath { get; set; }

        public virtual Projects Projects { get; set; }
    }
}
