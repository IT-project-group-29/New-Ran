using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace WebApplication4.Models
{
    public partial class Model1 : DbContext
    {
        public Model1()
            : base("name=Model1")
        {
        }

        public virtual DbSet<AspNetUserRoles> AspNetUserRoles { get; set; }
        public virtual DbSet<Applications> Applications { get; set; }
        public virtual DbSet<AspNetRoles> AspNetRoles { get; set; }
        public virtual DbSet<AspNetUserClaims> AspNetUserClaims { get; set; }
        public virtual DbSet<AspNetUserLogins> AspNetUserLogins { get; set; }
        public virtual DbSet<AspNetUsers> AspNetUsers { get; set; }
        public virtual DbSet<Clients> Clients { get; set; }
        public virtual DbSet<Course> Course { get; set; }
        public virtual DbSet<Memberships> Memberships { get; set; }
        public virtual DbSet<PeopleComments> PeopleComments { get; set; }
        public virtual DbSet<PeopleContacts> PeopleContacts { get; set; }
        public virtual DbSet<PlanCourses> PlanCourses { get; set; }
        public virtual DbSet<Plans> Plans { get; set; }
        public virtual DbSet<PriorityProjects> PriorityProjects { get; set; }
        public virtual DbSet<Profiles> Profiles { get; set; }
        public virtual DbSet<Program> Program { get; set; }
        public virtual DbSet<ProjectDocuments> ProjectDocuments { get; set; }
        public virtual DbSet<ProjectEfforts> ProjectEfforts { get; set; }
        public virtual DbSet<ProjectEffortsApplied> ProjectEffortsApplied { get; set; }
        public virtual DbSet<ProjectMethods> ProjectMethods { get; set; }
        public virtual DbSet<ProjectMethodsApplied> ProjectMethodsApplied { get; set; }
        public virtual DbSet<ProjectPeopleAllocations> ProjectPeopleAllocations { get; set; }
        public virtual DbSet<Projects> Projects { get; set; }
        public virtual DbSet<ProjectStatus> ProjectStatus { get; set; }
        public virtual DbSet<Roles> Roles { get; set; }
        public virtual DbSet<Staff> Staff { get; set; }
        public virtual DbSet<StudentCourses> StudentCourses { get; set; }
        public virtual DbSet<StudentProjectRanking> StudentProjectRanking { get; set; }
        public virtual DbSet<Students> Students { get; set; }
        public virtual DbSet<Users> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Applications>()
                .HasMany(e => e.Memberships)
                .WithRequired(e => e.Applications)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Applications>()
                .HasMany(e => e.Roles)
                .WithRequired(e => e.Applications)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Applications>()
                .HasMany(e => e.Users)
                .WithRequired(e => e.Applications)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<AspNetRoles>()
                .HasMany(e => e.AspNetUsers)
                .WithMany(e => e.AspNetRoles)
                .Map(m => m.ToTable("AspNetUserRoles").MapLeftKey("RoleId").MapRightKey("UserId"));

            modelBuilder.Entity<AspNetUsers>()
                .HasMany(e => e.AspNetUserClaims)
                .WithRequired(e => e.AspNetUsers)
                .HasForeignKey(e => e.UserId);

            modelBuilder.Entity<AspNetUsers>()
                .HasMany(e => e.AspNetUserLogins)
                .WithRequired(e => e.AspNetUsers)
                .HasForeignKey(e => e.UserId);

            modelBuilder.Entity<Clients>()
                .Property(e => e.companyName)
                .IsUnicode(false);

            modelBuilder.Entity<Course>()
                .Property(e => e.courseID)
                .IsUnicode(false);

            modelBuilder.Entity<Course>()
                .Property(e => e.courseName)
                .IsUnicode(false);

            modelBuilder.Entity<Course>()
                .Property(e => e.courseCode)
                .IsUnicode(false);

            modelBuilder.Entity<Course>()
                .Property(e => e.courseAbbreviation)
                .IsUnicode(false);

            modelBuilder.Entity<Course>()
                .HasMany(e => e.PlanCourses)
                .WithRequired(e => e.Course)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<PeopleComments>()
                .Property(e => e.comment)
                .IsUnicode(false);

            modelBuilder.Entity<PeopleContacts>()
                .Property(e => e.contactMethod)
                .IsUnicode(false);

            modelBuilder.Entity<PeopleContacts>()
                .Property(e => e.contactValue)
                .IsUnicode(false);

            modelBuilder.Entity<PlanCourses>()
                .Property(e => e.courseId)
                .IsUnicode(false);

            modelBuilder.Entity<PlanCourses>()
                .Property(e => e.temp)
                .IsFixedLength();

            modelBuilder.Entity<Plans>()
                .Property(e => e.planName)
                .IsUnicode(false);

            modelBuilder.Entity<Plans>()
                .Property(e => e.planCode)
                .IsUnicode(false);

            modelBuilder.Entity<Plans>()
                .HasMany(e => e.PlanCourses)
                .WithRequired(e => e.Plans)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Plans>()
                .HasMany(e => e.Students)
                .WithRequired(e => e.Plans)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<PriorityProjects>()
                .Property(e => e.priorityLevel)
                .IsUnicode(false);

            modelBuilder.Entity<PriorityProjects>()
                .Property(e => e.priorityReason)
                .IsUnicode(false);

            modelBuilder.Entity<Program>()
                .Property(e => e.programCode)
                .IsUnicode(false);

            modelBuilder.Entity<Program>()
                .Property(e => e.programDescription)
                .IsUnicode(false);

            modelBuilder.Entity<Program>()
                .Property(e => e.programDirectorName)
                .IsUnicode(false);

            modelBuilder.Entity<ProjectDocuments>()
                .Property(e => e.documentSource)
                .IsUnicode(false);

            modelBuilder.Entity<ProjectDocuments>()
                .Property(e => e.documentLink)
                .IsUnicode(false);

            modelBuilder.Entity<ProjectDocuments>()
                .Property(e => e.documentTitle)
                .IsUnicode(false);

            modelBuilder.Entity<ProjectDocuments>()
                .Property(e => e.filePath)
                .IsUnicode(false);

            modelBuilder.Entity<ProjectEfforts>()
                .Property(e => e.effortDescription)
                .IsUnicode(false);

            modelBuilder.Entity<ProjectEfforts>()
                .HasMany(e => e.ProjectEffortsApplied)
                .WithRequired(e => e.ProjectEfforts)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ProjectEffortsApplied>()
                .Property(e => e.comment)
                .IsUnicode(false);

            modelBuilder.Entity<ProjectMethods>()
                .Property(e => e.methodDescription)
                .IsUnicode(false);

            modelBuilder.Entity<ProjectMethods>()
                .HasMany(e => e.ProjectMethodsApplied)
                .WithRequired(e => e.ProjectMethods)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ProjectMethodsApplied>()
                .Property(e => e.comment)
                .IsUnicode(false);

            modelBuilder.Entity<ProjectPeopleAllocations>()
                .Property(e => e.personRole)
                .IsUnicode(false);

            modelBuilder.Entity<ProjectPeopleAllocations>()
                .Property(e => e.creatorComment)
                .IsUnicode(false);

            modelBuilder.Entity<Projects>()
                .Property(e => e.projectCode)
                .IsUnicode(false);

            modelBuilder.Entity<Projects>()
                .Property(e => e.projectTitle)
                .IsUnicode(false);

            modelBuilder.Entity<Projects>()
                .Property(e => e.projectScope)
                .IsUnicode(false);

            modelBuilder.Entity<Projects>()
                .Property(e => e.projectOutcomes)
                .IsUnicode(false);

            modelBuilder.Entity<Projects>()
                .Property(e => e.projectPlacementRequirements)
                .IsUnicode(false);

            modelBuilder.Entity<Projects>()
                .Property(e => e.projectStatusComment)
                .IsUnicode(false);

            modelBuilder.Entity<Projects>()
                .Property(e => e.projectSemester)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Projects>()
                .Property(e => e.projectSemesterCode)
                .IsUnicode(false);

            modelBuilder.Entity<Projects>()
                .Property(e => e.honoursUndergrad)
                .IsUnicode(false);

            modelBuilder.Entity<Projects>()
                .Property(e => e.projectEffortRequirements)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Projects>()
                .Property(e => e.scholarshipDetail)
                .IsUnicode(false);

            modelBuilder.Entity<Projects>()
                .HasMany(e => e.ProjectDocuments)
                .WithRequired(e => e.Projects)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Projects>()
                .HasMany(e => e.ProjectEffortsApplied)
                .WithRequired(e => e.Projects)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Projects>()
                .HasMany(e => e.ProjectMethodsApplied)
                .WithRequired(e => e.Projects)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Projects>()
                .HasMany(e => e.StudentProjectRanking)
                .WithRequired(e => e.Projects)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ProjectStatus>()
                .Property(e => e.StatusName)
                .IsUnicode(false);

            modelBuilder.Entity<ProjectStatus>()
                .Property(e => e.StatusDescription)
                .IsUnicode(false);

            modelBuilder.Entity<ProjectStatus>()
                .HasMany(e => e.Projects)
                .WithRequired(e => e.ProjectStatus1)
                .HasForeignKey(e => e.projectStatus);

            modelBuilder.Entity<Roles>()
                .HasMany(e => e.Users)
                .WithMany(e => e.Roles)
                .Map(m => m.ToTable("UsersInRoles").MapLeftKey("RoleId").MapRightKey("UserId"));

            modelBuilder.Entity<Staff>()
                .Property(e => e.uniStaffID)
                .IsUnicode(false);

            modelBuilder.Entity<Staff>()
                .Property(e => e.username)
                .IsUnicode(false);

            modelBuilder.Entity<StudentCourses>()
                .Property(e => e.courseID)
                .IsUnicode(false);

            modelBuilder.Entity<StudentCourses>()
                .Property(e => e.semester)
                .IsUnicode(false);

            modelBuilder.Entity<StudentCourses>()
                .Property(e => e.grade)
                .IsUnicode(false);

            modelBuilder.Entity<StudentCourses>()
                .Property(e => e.mark)
                .HasPrecision(4, 1);

            modelBuilder.Entity<Students>()
                .Property(e => e.uniUserName)
                .IsUnicode(false);

            modelBuilder.Entity<Students>()
                .Property(e => e.uniStudentID)
                .IsUnicode(false);

            modelBuilder.Entity<Students>()
                .Property(e => e.gpa)
                .HasPrecision(3, 2);

            modelBuilder.Entity<Students>()
                .Property(e => e.genderCode)
                .IsUnicode(false);

            modelBuilder.Entity<Students>()
                .Property(e => e.international)
                .IsUnicode(false);

            modelBuilder.Entity<Students>()
                .Property(e => e.studentEmail)
                .IsUnicode(false);

            modelBuilder.Entity<Students>()
                .Property(e => e.semester)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Students>()
                .HasMany(e => e.StudentProjectRanking)
                .WithRequired(e => e.Students)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Users>()
                .HasOptional(e => e.Memberships)
                .WithRequired(e => e.Users);

            modelBuilder.Entity<Users>()
                .HasOptional(e => e.Profiles)
                .WithRequired(e => e.Users);
        }
    }
}
