namespace WebApplication4.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Project
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Project()
        {
            PriorityProjects = new HashSet<PriorityProject>();
            ProjectDocuments = new HashSet<ProjectDocument>();
            ProjectEffortsApplieds = new HashSet<ProjectEffortsApplied>();
            ProjectMethodsApplieds = new HashSet<ProjectMethodsApplied>();
            ProjectPeopleAllocations = new HashSet<ProjectPeopleAllocation>();
            StudentProjectRankings = new HashSet<StudentProjectRanking>();
        }

        [Required]
        [StringLength(128)]
        public string Id { get; set; }

        public int projectID { get; set; }

        [StringLength(15)]
        public string projectCode { get; set; }

        [StringLength(250)]
        public string projectTitle { get; set; }

        public string projectScope { get; set; }

        public string projectOutcomes { get; set; }

        public int projectDuration { get; set; }

        [StringLength(500)]
        public string projectPlacementRequirements { get; set; }

        public bool projectSponsorAgreement { get; set; }

        public int projectStatus { get; set; }

        [StringLength(500)]
        public string projectStatusComment { get; set; }

        public DateTime? projectStatusChangeDate { get; set; }

        [StringLength(3)]
        public string projectSemester { get; set; }

        [StringLength(10)]
        public string projectSemesterCode { get; set; }

        public int projectYear { get; set; }

        public int? projectSequenceNo { get; set; }

        [Required]
        [StringLength(5)]
        public string honoursUndergrad { get; set; }

        public bool requirementsMet { get; set; }

        public int projectCreatorID { get; set; }

        public DateTime dateCreated { get; set; }

        [StringLength(10)]
        public string projectEffortRequirements { get; set; }

        public bool austCitizenOnly { get; set; }

        public int studentsReq { get; set; }

        public int? scholarshipAmt { get; set; }

        public string scholarshipDetail { get; set; }

        public DateTime? staffEmailSentDate { get; set; }

        public DateTime? clientEmailSentDate { get; set; }

        public DateTime? studentEmailSentDate { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PriorityProject> PriorityProjects { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProjectDocument> ProjectDocuments { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProjectEffortsApplied> ProjectEffortsApplieds { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProjectMethodsApplied> ProjectMethodsApplieds { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProjectPeopleAllocation> ProjectPeopleAllocations { get; set; }

        public virtual ProjectStatu ProjectStatu { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<StudentProjectRanking> StudentProjectRankings { get; set; }
    }
}
