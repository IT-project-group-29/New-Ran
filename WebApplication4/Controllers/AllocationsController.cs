using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplication4.Models;

namespace WebApplication4.Controllers
{
    
    public class AllocationsController : Controller
    {

        private Model1 db = new Model1();
        // GET: Allocations
        public ActionResult Index()
        {

           
            ViewBag.ddlyear = DateNow();
            ViewBag.DDLSemester = SemNow();
            //Pass the time for the view, and the default is the current time

            var sem = SemNow();
            ViewBag.project = GetProject(DateTime.Now.Year, SemNow().SelectedValue.ToString());
            ViewBag.pstatus = db.ProjectStatus.ToList();
            ViewBag.staff = db.Staff.ToList();
            ViewBag.stdt = db.Students.ToList();
            ViewBag.plans = db.Plans.ToList();
            ViewBag.plansID = GetAllPlansID();
            var projectPeopleAllocations = db.ProjectPeopleAllocations.Include(p => p.Projects);
            
            return View(projectPeopleAllocations.ToList());
        }
        public SelectList PlanSel()
        {
            var plans = new List<string>();
            return null;
        }
        public SelectList DateNow()
        {
            var ddlyear = new List<string>();
            
            //create lists to put semester in.
            var currentDate = System.DateTime.Now;
            //create lists to put years in.

            for (int i = -2; i <= 2; i++)
            {
                ddlyear.Add(currentDate.AddYears(i).Year.ToString());
                //add years, Based on the current time

            }


            int yearNow = DateTime.Now.Year;
            int selectyear = 0;
            for (int i = 0; i < ddlyear.Count - 1; i++)
            {
                if (Convert.ToInt32(ddlyear[i]) == yearNow)
                {
                    selectyear = i;
                }
            }
            //Find the location in the list where the current time is stored.
          

            
            //to judge which semester users are in now
            return new SelectList(ddlyear, ddlyear[selectyear]);
        }
        public SelectList SemNow()
        {
            var DDLSemester = new List<string>();
            DDLSemester.Add("SP2");
            DDLSemester.Add("SP5");

            int selectMonth = 0;
            if (DateTime.Now.Month < 6)
            {
                selectMonth = 0;
            }
            else
            {
                selectMonth = 1;
            }
            return new SelectList(DDLSemester, DDLSemester[selectMonth]);
        }



        public List<int> GetAllPlansID()
        {//use this func can get all plans' ID in database
            var l = new List<int>();
            var a = db.Plans.ToList();
            foreach (var item in a)
            {
                l.Add(item.planId);
            }
            return l;
        }
       public List<string> Plan_course(int planid)
        {//use this func can get a List which store course ID by  planID
            var l = new List<string>();
            var a = db.PlanCourses.Where(p => p.planId == planid).ToList();
            foreach(var item in a)
            {
                l.Add(item.courseId);
            }
            return l;
        }
        public string CourseName(string courseId)
        {//get course name by course ID
            var a = db.Course.FirstOrDefault(p => p.courseID.Equals(courseId)).courseName;
            return a;
        }

        public ActionResult ChangeStatus(string projId, string statusId)
        { var pid = Convert.ToInt32(projId);
            var statuID = db.ProjectStatus.FirstOrDefault(a => a.StatusName == statusId).ProjectStatusId;
            var proj = db.Projects.FirstOrDefault(a => a.projectID == pid);
            proj.projectStatus = statuID;
            proj.projectStatusChangeDate = DateTime.Now;

            db.SaveChanges();
            return Content("OK");
        }

        public ActionResult Change(string pop,string YearBeSel, string SemBeSel)
        {var year = Convert.ToInt32(YearBeSel);
            ViewBag.staff = db.Staff.OrderBy(a => a.username).ToList();
            ViewBag.stdt = db.Students.OrderBy(a => a.uniUserName).ToList();
            ViewBag.project = GetProject(year, SemBeSel);
            var projectPeopleAllocations = db.ProjectPeopleAllocations.Include(p => p.Projects);
            var staffAllocations = db.ProjectPeopleAllocations.Where(a => a.personRole == "staff");
            if (pop == "staff")
            {
                
                return PartialView("staffTable", staffAllocations.ToList());
            }
            else
            {
                return PartialView("AllPlanStudent", projectPeopleAllocations.ToList());
                //this function is used to Asynchronous refresh people table between staff and student
            }

        }

        public ActionResult SelectPlan(string planSelecter)
        {
            var projectPeopleAllocations = db.ProjectPeopleAllocations.Include(p => p.Projects);
            if (planSelecter == "AllPlan")
            {             
                ViewBag.stdt = db.Students.OrderBy(a => a.uniUserName).ToList();
                return PartialView("AllPlanStudent", projectPeopleAllocations.ToList());
            }
            else
            {
                var planSel = Convert.ToInt32(planSelecter);
                ViewBag.stdt = db.Students.Where(a => a.planId == planSel).ToList();
                ViewBag.stdtplanId = planSel;
                return PartialView("Selectplan", projectPeopleAllocations.ToList());
            }
            //when select a new plan, the sort select's option will change, the new data is from database
        }
        public ActionResult ChangeDate(int ddlyear, string DDLSemester)
        {
            var projectPeopleAllocations = db.ProjectPeopleAllocations.Include(p => p.Projects);
            var projects = GetProject(ddlyear, DDLSemester);
            
            ViewBag.project = projects;
            
            ViewBag.pstatus = db.ProjectStatus.ToList();
            ViewBag.staff = db.Staff.ToList();
            ViewBag.stdt = db.Students.ToList();
            return PartialView("ProjectAllocation", projectPeopleAllocations.ToList());
            //change date and find the select semester and year's projects
        }
        public List<Projects> GetProject(int ddlyear, string DDLSemester)
        {
            var projects = new List<Projects>();
            if (DDLSemester == "SP5")
            {
                foreach (var project in db.Projects.Where(p => p.projectYear <= ddlyear && p.projectYear + p.projectDuration > ddlyear))
                {
                    projects.Add(project);
                }
               

            }
            if (DDLSemester == "SP2")
            {
                foreach (var project in db.Projects.Where(p => p.projectYear <= ddlyear && p.projectYear + p.projectDuration > ddlyear))
                {
                    projects.Add(project);
                }
                foreach (var project in db.Projects.Where(p => p.projectYear < ddlyear && p.projectYear + p.projectDuration == ddlyear && p.projectSemester == "SP5"))
                {
                    projects.Add(project);
                }

            }
            return projects;
        }

        public ActionResult EditStaff(int STID)
        {
            var a = db.Staff.FirstOrDefault(c => c.staffID == STID);
            ViewBag.stff = a;
            return PartialView("staffDiv");
            //return a selected staff information's view to Asynchronous refresh the <div> which display it
        }
        public ActionResult ChangeSortPlan(string SelecedPlan)
        {
            if(SelecedPlan == "AllPlan")
            {
                

                return PartialView("AllPlanSort");
            }
            else
            {
                int SelPlan = Convert.ToInt32(SelecedPlan);
                ViewBag.CourseID = db.PlanCourses.Where(a => a.planId == SelPlan).ToList();
                ViewBag.Course = db.Course.ToList();
                return PartialView("SelectedSort");
            }
            //when select a new plan, the student table will change, add new data"grade"
        }

        public ActionResult OrderByFunc(string OrderBySort,string OrderByPlanId, string OrderBySortCourseId)
        {
            //this function is to sort order student table, there are some different situations need to make sure by the value
            var projectPeopleAllocations = db.ProjectPeopleAllocations.Include(p => p.Projects);
            if (OrderByPlanId != "AllPlan")
            {
                int planid = Convert.ToInt32(OrderByPlanId.Substring(4));
                var studentslist = db.Students.Where(a => a.planId == planid).ToList();
                List<StudentCourses> studentcoureselist = new List<StudentCourses>();
               
            
           
            if (OrderBySort == "Positive")
            {
                if (OrderBySortCourseId == "Name")
                {
                    ViewBag.stdt = db.Students.Where(a => a.planId == planid).OrderBy(a => a.uniUserName).ToList();
                        
                        return PartialView("AllPlanStudent", projectPeopleAllocations.ToList());
                }
                else if (OrderBySortCourseId == "GPA")
                {
                    ViewBag.stdt = db.Students.Where(a => a.planId == planid).OrderByDescending(a => a.gpa).ToList();
                       
                        return PartialView("AllPlanStudent", projectPeopleAllocations.ToList());
                }
                else
                {
                        foreach (var item in studentslist)
                        {
                            StudentCourses studentcourses = db.StudentCourses.Where(x => x.studentID == item.studentID && x.courseID == OrderBySortCourseId).FirstOrDefault();
                            studentcoureselist.Add(studentcourses);
                        }

                        ViewBag.stdtCs = studentcoureselist.OrderByDescending(a => a.grade).ToList();
                        ViewBag.stdt = db.Students.Where(a => a.planId == planid).ToList();
                       
                        return PartialView("StudentOrderByCourseGrade", projectPeopleAllocations.ToList());

                    }
            }if(OrderBySort == "Negative")
            {
                if (OrderBySortCourseId == "Name")
                {
                    ViewBag.stdt = db.Students.Where(a => a.planId == planid).OrderByDescending(a => a.uniUserName).ToList();
                        
                        return PartialView("AllPlanStudent", projectPeopleAllocations.ToList());
                }
                else if (OrderBySortCourseId == "GPA")
                {
                    ViewBag.stdt = db.Students.Where(a => a.planId == planid).OrderBy(a => a.gpa).ToList();
                        
                        return PartialView("AllPlanStudent", projectPeopleAllocations.ToList());
                }
                else
                {
                        foreach (var item in studentslist)
                        {
                            StudentCourses studentcourses = db.StudentCourses.Where(x => x.studentID == item.studentID && x.courseID == OrderBySortCourseId).FirstOrDefault();
                            studentcoureselist.Add(studentcourses);
                        }

                        ViewBag.stdtCs = studentcoureselist.OrderBy(a => a.grade).ToList();
                        ViewBag.stdt = db.Students.Where(a => a.planId == planid).ToList();
                        
                        return PartialView("StudentOrderByCourseGrade", projectPeopleAllocations.ToList());
                    }
            }
            }
            if (OrderByPlanId == "AllPlan")
            {
               /* int planid = Convert.ToInt32(OrderByPlanId);
                var studentslist = db.Students.Where(a => a.planId == planid).ToList();
                List<StudentCourses> studentcoureselist = new List<StudentCourses>();
                foreach (var item in studentslist)
                {
                    StudentCourses studentcourses = db.StudentCourses.Where(x => x.studentID == item.studentID).FirstOrDefault();
                    studentcoureselist.Add(studentcourses);
                }*/



                if (OrderBySort == "Positive")
                {
                    if (OrderBySortCourseId == "Name")
                    {
                        ViewBag.stdt = db.Students.OrderBy(a => a.uniUserName).ToList();
                        
                        return PartialView("AllPlanStudent", projectPeopleAllocations.ToList());
                    }
                    else if (OrderBySortCourseId == "GPA")
                    {
                        ViewBag.stdt = db.Students.OrderByDescending(a => a.gpa).ToList();
                        
                        return PartialView("AllPlanStudent", projectPeopleAllocations.ToList());
                    }
                    
                }
                if (OrderBySort == "Negative")
                {
                    if (OrderBySortCourseId == "Name")
                    {
                        ViewBag.stdt = db.Students.OrderByDescending(a => a.uniUserName).ToList();
                       
                        return PartialView("AllPlanStudent", projectPeopleAllocations.ToList());
                    }
                    else if (OrderBySortCourseId == "GPA")
                    {
                        ViewBag.stdt = db.Students.OrderBy(a => a.gpa).ToList();
                        
                        return PartialView("AllPlanStudent", projectPeopleAllocations.ToList());
                    }
                   
                }
            }

            return PartialView();
        }


        public ActionResult DeleteStudentAllocation(string personId, string projectId)
        {// when drag student back from project table, will use this function to delete the ProjectPeopleAllocation data
            string planid = "";
            var Person = Convert.ToInt32(personId);
            var Proj = Convert.ToInt32(projectId);
            var DeleteAllocation = db.ProjectPeopleAllocations.FirstOrDefault(a => a.personID == Person && a.projectID == Proj);
            if(DeleteAllocation.personRole == "student")
            {
                planid = db.Students.FirstOrDefault(a => a.studentID == Person).planId.ToString();
            }
            db.ProjectPeopleAllocations.Remove(DeleteAllocation);
            db.SaveChanges();
            
            
            return Content(planid);
        }

        public ActionResult DeleteStaffAllocation(string personId, string projectId)
        {
            // when drag staff back from project table, will use this function to delete the ProjectPeopleAllocation data
            var Person = Convert.ToInt32(personId);
            var Proj = Convert.ToInt32(projectId);
            var DeleteAllocation = db.ProjectPeopleAllocations.FirstOrDefault(a => a.personID == Person && a.projectID == Proj);
            
            db.ProjectPeopleAllocations.Remove(DeleteAllocation);
            db.SaveChanges();


            return Content("OK");
        }

        public ActionResult AddStudentToProject(string studentId,string projectId)
        {//when drag student from student table to project table, use this func to add new ProjectPeopleAllocation data
            var PPA = new ProjectPeopleAllocations();
            var userid = Convert.ToInt32(studentId);
            PPA.projectID = Convert.ToInt32( projectId);
            PPA.personID = userid;
            PPA.personRole = "student";
            PPA.dateCreated = DateTime.Now;

            db.ProjectPeopleAllocations.Add(PPA);          

            db.SaveChanges();
            var name = db.Students.FirstOrDefault(a => a.studentID == userid).uniUserName;
            return Content(name); 
        }
        public ActionResult AddStaffToProject(string staffId,string projectId)
        {//when drag staff from staff table to project table, use this func to add new ProjectPeopleAllocation data
            var PPA = new ProjectPeopleAllocations();
            var userid = Convert.ToInt32(staffId);
            var project = Convert.ToInt32(projectId);
            if (db.ProjectPeopleAllocations.FirstOrDefault(a => a.projectID == project && a.personID == userid) == null)
            {

            
            PPA.projectID = Convert.ToInt32(projectId);
            PPA.personID = userid;
            PPA.personRole = "staff";
            PPA.dateCreated = DateTime.Now;

            db.ProjectPeopleAllocations.Add(PPA);

            db.SaveChanges();
            var name = db.Staff.FirstOrDefault(a => a.staffID == userid).username;
            return Content(name);
            }
            return Content("Seleced staff is already in this project");
        }

        //==================================================================================================================
        public ActionResult StaffProjectIndex()
        {
            ViewBag.staff = db.Staff.OrderBy(a => a.username).ToList();
            return View(db.ProjectPeopleAllocations.ToList());
        }

    }
}