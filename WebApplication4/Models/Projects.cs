namespace WebApplication4.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Projects
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Projects()
        {
            PriorityProjects = new HashSet<PriorityProjects>();
            ProjectDocuments = new HashSet<ProjectDocuments>();
            ProjectEffortsApplied = new HashSet<ProjectEffortsApplied>();
            ProjectMethodsApplied = new HashSet<ProjectMethodsApplied>();
            ProjectPeopleAllocations = new HashSet<ProjectPeopleAllocations>();
            StudentProjectRanking = new HashSet<StudentProjectRanking>();
        }

        [Required]
        [StringLength(128)]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Key]
        public int projectID { get; set; }

        [StringLength(15)]
        [Required]
        public string projectCode { get; set; } = "";

        [StringLength(250)]
        [Required]
        public string projectTitle { get; set; } = "";

        public string projectScope { get; set; } = "";

        public string projectOutcomes { get; set; } = "";

        public int projectDuration { get; set; } = 0;

        [StringLength(500)]
        public string projectPlacementRequirements { get; set; } = "";

        public bool projectSponsorAgreement { get; set; } = false;

        public int projectStatus { get; set; } = 0;

        [StringLength(500)]
        public string projectStatusComment { get; set; } = "";

        public DateTime? projectStatusChangeDate { get; set; } = DateTime.Now;

        [StringLength(3)]
        public string projectSemester { get; set; } = "";

        [StringLength(10)]
        public string projectSemesterCode { get; set; } = "";

        public int projectYear { get; set; } = 0;

        public int? projectSequenceNo { get; set; } = 0;

        [Required]
        [StringLength(5)]
        public string honoursUndergrad { get; set; } = "";

        public bool requirementsMet { get; set; } = false;

        public int projectCreatorID { get; set; } = 0;

        public DateTime dateCreated { get; set; }=DateTime.Now;

        [StringLength(10)]
        public string projectEffortRequirements { get; set; } = "";

        public bool austCitizenOnly { get; set; } = false;

        public int studentsReq { get; set; }

        public int? scholarshipAmt { get; set; }

        public string scholarshipDetail { get; set; } = "";

        public DateTime? staffEmailSentDate { get; set; }

        public DateTime? clientEmailSentDate { get; set; }

        public DateTime? studentEmailSentDate { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PriorityProjects> PriorityProjects { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProjectDocuments> ProjectDocuments { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProjectEffortsApplied> ProjectEffortsApplied { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProjectMethodsApplied> ProjectMethodsApplied { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProjectPeopleAllocations> ProjectPeopleAllocations { get; set; }

        public virtual ProjectStatus ProjectStatus1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<StudentProjectRanking> StudentProjectRanking { get; set; }
    }
}
