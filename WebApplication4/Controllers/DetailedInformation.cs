using Microsoft.Ajax.Utilities;
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
    public class DetailedInformationController : Controller
    {
        private Model1 db = new Model1();

        // GET: DetailedInformation
        public ActionResult Index(int? index=0, string orderby="", string orderby1="",string sortby = "")
        {
            ViewBag.stdt = db.Students.ToList();
            ViewBag.plans = db.Plans.ToList();
            ViewBag.plansID = index;
            ViewBag.course = db.Course.ToList();
            ViewBag.OrderBy = orderby;
            if(orderby != orderby1)
            {
                sortby = "";
            }
            ViewBag.SortBy = sortby == ""?"desc":"";
            ViewBag.plancs = db.PlanCourses.ToList();
            List<StudentInfo> StdInfom = GetStudents();
            ViewBag.stcs = StdInfom;



            List<string> PlanCourses = new List<string>();

            if (index == 0 || index == null)
            {
                PlanCourses = db.Course.OrderBy(s => s.courseID).Select(s=>s.courseName).ToList();
            }
            else
            {
                var plancourses = db.PlanCourses.Include(a => a.Course).Where(a => a.planId == index).Select(a => a.Course).OrderBy(s=>s.courseID).ToList();
                PlanCourses = plancourses.Select(a => a.courseName).ToList();
            }
            PlanCourses.Insert(0, "Name");
            ViewBag.PlanCourse = PlanCourses.ToList();
            List<Students> thisPlanStudent = new List<Students>();
            List<CourseModel> CourseList = new List<CourseModel>();

            if (index == 0 || index == null)
            {
                thisPlanStudent = db.Students.ToList();
                var courses = db.Course.OrderBy(s=>s.courseID).ToList();
                foreach(var s in thisPlanStudent)
                {
                    var myModel = new CourseModel
                    {
                        Name = s.uniUserName,
                        
                    };
                    List<StudentCourses> studentCourses = new List<StudentCourses>();
                    foreach (var n in courses)
                    {
                        StudentCourses studentCourses1 = new StudentCourses();
                        var courseStudents = db.StudentCourses.Where(c => c.courseID == n.courseID && c.Students.studentID == s.studentID).FirstOrDefault();
                        if (courseStudents != null)
                        {
                            studentCourses.Add(courseStudents);
                        } else
                        {
                            studentCourses1.grade = "";
                            studentCourses1.courseID = n.courseID;
                            studentCourses1.studentID = s.studentID;
                            studentCourses.Add(studentCourses1);
                        }

                                          
                    }
                    myModel.StudentCourses = studentCourses;
                    CourseList.Add(myModel);

                }

            }
            else
            {
                thisPlanStudent = db.Students.Where(n => n.planId == index).ToList();
                foreach (var n in thisPlanStudent)
                {
                    var courseStudents = db.StudentCourses.Where(m => m.studentID == n.studentID).OrderBy(s => s.courseID).ToList();

                    var myModel = new CourseModel
                    {
                        Name = n.uniUserName,
                        StudentCourses = courseStudents
                    };
                    CourseList.Add(myModel);
                }
            }

            //thisPlanStudent= thisPlanStudent.OrderBy(s => s.StudentCourses.Where(d=>d.Course.courseName == "" ).Select(d=>d.grade)).ToList();
            
            //if the sortby is not empty,sort
            if(!string.IsNullOrEmpty(orderby))
            {

                //If sortby is empty, it will be in ascending order, and orderby is name
                if (orderby == "Name" &&  sortby == "")
                {
                    CourseList = CourseList.OrderBy(s=>s.Name).ToList();
                } else if (orderby == "Name" && sortby == "desc")
                {
                    CourseList = CourseList.OrderByDescending(s => s.Name).ToList();
                } else
                {
                    var grad = db.StudentCourses.Where(d => d.Course.courseName == orderby && d.Students.uniUserName != "" && d.Students.planId == 0).ToList();
                    if (sortby == "") //Ascending order
                    {
                        CourseList = CourseList.OrderBy(s => s.StudentCourses.Where(d =>d.Course!=null&& d.Course.courseName == orderby ).FirstOrDefault()).ToList();
                    } else if(sortby == "desc") //Descending order
                    {
                        CourseList = CourseList.OrderByDescending(s => s.StudentCourses.Where(d => d.Course != null && d.Course.courseName == orderby).FirstOrDefault()).ToList();
                    }
                }
                
                       }

            ViewBag.CourseList = CourseList.DistinctBy(a => a.Name).ToList();
            var projectPeopleAllocations = db.ProjectPeopleAllocations.Include(p => p.Projects);

            return View(projectPeopleAllocations.ToList());
        }

        private List<StudentInfo> GetStudents()
        {
            List<StudentInfo> StdInfom = new List<StudentInfo>();
            foreach (var item in db.Students)
            {
                StdInfom.Add(new StudentInfo
                {
                    ID = item.studentID,
                    Planid = item.planId,
                    Name = item.uniUserName,
                    Gpa = item.gpa,
                }); ;
            }
            return StdInfom;
        }



    }

}