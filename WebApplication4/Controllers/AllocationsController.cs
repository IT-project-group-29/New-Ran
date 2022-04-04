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
            ViewBag.project = db.Projects.Where(p => p.projectYear == DateTime.Now.Year && p.projectSemester == "SP2").ToList();
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
        public ActionResult Change(string pop)
        {
            ViewBag.staff = db.Staff.ToList();
            ViewBag.stdt = db.Students.ToList();
            if (pop == "staff")
            {
                
                return PartialView("staffTable");
            }
            else
            {
                return PartialView("AllPlanStudent");
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
        }
        public ActionResult ChangeDate(int ddlyear, string DDLSemester)
        {
            var projectPeopleAllocations = db.ProjectPeopleAllocations.Include(p => p.Projects);
            ViewBag.project = db.Projects.Where(p => p.projectYear == ddlyear && p.projectSemester == DDLSemester).ToList();
            ViewBag.pstatus = db.ProjectStatus.ToList();
            ViewBag.staff = db.Staff.ToList();
            ViewBag.stdt = db.Students.ToList();
            return PartialView("ProjectAllocation", projectPeopleAllocations.ToList());
        }

        public ActionResult EditStaff(int STID)
        {
            var a = db.Staff.FirstOrDefault(c => c.staffID == STID);
            ViewBag.stff = a;
            return PartialView("staffDiv");
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
        }

        public ActionResult OrderByFunc(string OrderBySort,string OrderByPlanId, string OrderBySortCourseId)
        {
            var projectPeopleAllocations = db.ProjectPeopleAllocations.Include(p => p.Projects);
            if (OrderByPlanId != "AllPlan")
            {
                int planid = Convert.ToInt32(OrderByPlanId.Substring(5));
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

        public ActionResult AddStudentToProject(string studentId,string projectId)
        {
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

    }
}