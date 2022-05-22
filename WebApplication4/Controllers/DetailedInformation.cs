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
        public ActionResult Index(int? index = 0, int yearid = 0, int semesterid = 0, int courseid = 0, string orderby = "", string orderby1 = "", string sortby = "", int flag = 0)
        {
            ViewBag.stdt = db.Students.ToList();
            ViewBag.plans = db.Plans.ToList();
            ViewBag.plansID = index;
            ViewBag.course = db.Course.ToList();
            ViewBag.OrderBy = orderby;
            if (orderby != orderby1)
            {
                sortby = "";
            }
            ViewBag.SortBy = sortby == "" ? "desc" : "";
            ViewBag.plancs = db.PlanCourses.ToList();
            var courseyears = Years(yearid);
            var semesters = Semeters(semesterid);
            var courseslist = Courses(courseid);
            ViewBag.courseYears = courseyears;
            ViewBag.semesters = semesters;
            ViewBag.courses = courseslist;
            ViewBag.semesterid = semesterid;
            ViewBag.courseid = courseid;
            ViewBag.yearid = yearid;
            ViewBag.flag = flag;
            List<string> PlanCourses = new List<string>();
            Dictionary<string, string> PlanCoursesAbbr = new Dictionary<string, string>();
            List<CourseModel> CourseList = new List<CourseModel>();
            if (flag == 0)
            {
                if (index == 0 || index == null)
                {
                    var courseslists = db.Course.OrderBy(s => s.courseID).ToList();
                    foreach (var course in courseslists)
                    {
                        PlanCoursesAbbr[course.courseAbbreviation] = course.courseName;
                        PlanCourses.Add(course.courseAbbreviation);
                    }
                }
                else
                {
                    var plancourses = db.PlanCourses.Include(a => a.Course).Where(a => a.planId == index).Select(a => a.Course).OrderBy(s => s.courseID).ToList();
                    foreach (var course in plancourses)
                    {
                        PlanCoursesAbbr[course.courseAbbreviation] = course.courseName;
                        PlanCourses.Add(course.courseAbbreviation);
                    
                    }
                }
                PlanCourses.Insert(0, "Name");
                PlanCoursesAbbr["Name"] = "Name";
                ViewBag.PlanCourse = PlanCourses.ToList();
                ViewBag.PlanCourseDict = PlanCoursesAbbr;
                List<Students> thisPlanStudent = new List<Students>();


                if (index == 0 || index == null)
                {
                    thisPlanStudent = db.Students.ToList();
                    var courses = db.Course.OrderBy(s => s.courseID).ToList();
                    foreach (var s in thisPlanStudent)
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
                            }
                            else
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

            }
            else
            {
                var selectedcourse = courseslist.Where(s => s.Selected == true).FirstOrDefault()?.Text;
                var abbr = db.Course.Where(s => s.courseName == selectedcourse).FirstOrDefault()?.courseAbbreviation;
                var selectedyear = int.Parse(courseyears.Where(s => s.Selected == true).FirstOrDefault()?.Text);
                //let string to be int
                var selectedsemester = semesters.Where(s => s.Selected == true).FirstOrDefault()?.Text;
                if (selectedcourse != null && selectedyear != 0 && selectedsemester != null)
                {
                    var studentcourses = db.StudentCourses.Where(s => s.semester == selectedsemester && s.Course.courseName == selectedcourse && s.year == selectedyear).ToList();
                    PlanCourses.Add(abbr);
                    PlanCoursesAbbr[abbr] = selectedcourse;
                    PlanCourses.Insert(0, "Name");
                    PlanCoursesAbbr["Name"] = "Name";

                    ViewBag.PlanCourse = PlanCourses.ToList();
                    ViewBag.PlanCourseDict = PlanCoursesAbbr;
                    foreach (var studentcourse in studentcourses)
                    {
                        List<StudentCourses> studentCourses = new List<StudentCourses>();
                        studentCourses.Add(studentcourse);
                        var myModel = new CourseModel
                        {
                            Name = studentcourse.Students.uniUserName,
                            StudentCourses = studentCourses
                        };
                        CourseList.Add(myModel);
                    }

                }

            }
            //orderby
            if (!string.IsNullOrEmpty(orderby))
            {

                //If sortby is empty, it will be in ascending order, and orderby is name
                if (orderby == "Name" && sortby == "")
                {
                    CourseList = CourseList.OrderBy(s => s.Name).ToList();
                }
                else if (orderby == "Name" && sortby == "desc")
                {
                    CourseList = CourseList.OrderByDescending(s => s.Name).ToList();
                } 
                else
                {
                    var grad = db.StudentCourses.Where(d => d.Course.courseName == orderby && d.Students.uniUserName != "" && d.Students.planId == 0).ToList();
                    if (sortby == "") //Ascending order
                    {
                        CourseList = CourseList.OrderBy(s => s.StudentCourses.Where(d => d.Course != null && d.Course.courseName == orderby).FirstOrDefault()).ToList();
                    }
                    else if (sortby == "desc") //Descending order
                    {
                        CourseList = CourseList.OrderByDescending(s => s.StudentCourses.Where(d => d.Course != null && d.Course.courseName == orderby).FirstOrDefault()).ToList();
                    }
                }

            }

            ViewBag.CourseList = CourseList.DistinctBy(a => a.Name).ToList();

            return View();
        }


        public List<SelectListItem> Years(int index = 0)
            //select box for choose year
        {


            var courseYears = db.StudentCourses.OrderBy(s => s.year).Select(s => s.year).Distinct().ToList();
            List<SelectListItem> selectListItems = new List<SelectListItem>();
            int count = 0;
            foreach (var course in courseYears)
            {
                if (index == count)
                {
                    selectListItems.Add(new SelectListItem { Text = course.ToString(), Value = (count++).ToString(), Selected = true });

                }
                else
                {
                    selectListItems.Add(new SelectListItem { Text = course.ToString(), Value = (count++).ToString(), Selected = false });

                }
            }
            return selectListItems;
        }
        public List<SelectListItem> Semeters(int index = 0)
             //dropdownlist box for choose semester
        {
            var semesters = db.StudentCourses.OrderBy(s => s.semester).Select(s => s.semester).Distinct().ToList();
            List<SelectListItem> selectListItems = new List<SelectListItem>();
            int count = 0;
            foreach (var sem in semesters)
            {
                if (index == count)
                {
                    selectListItems.Add(new SelectListItem { Text = sem.ToString(), Value = (count++).ToString(), Selected = true });

                }
                else
                {
                    selectListItems.Add(new SelectListItem { Text = sem.ToString(), Value = (count++).ToString(), Selected = false });

                }
            }
            return selectListItems;
        }
      
        public List<SelectListItem> Courses(int index = 0)
        /*the selectbox of the courses and orderby the course name*/
        {
            var courses = db.Course.OrderBy(s => s.courseName).Select(s => s.courseName).ToList();
            List<SelectListItem> selectListItems = new List<SelectListItem>();

            int count = 0;
            foreach (var course in courses)
            {
                if (index == count)
                {
                    selectListItems.Add(new SelectListItem { Text = course.ToString(), Value = (count++).ToString(), Selected = true });

                }
                else
                {
                    selectListItems.Add(new SelectListItem { Text = course.ToString(), Value = (count++).ToString(), Selected = false });

                }
            }
            return selectListItems;
        }

    }

}