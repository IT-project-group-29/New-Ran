using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplication4.Models;
using WebApplication4.Util;

namespace WebApplication4.Controllers
{
    public class StudentCoursesController : Controller
    {
        private Model1 db = new Model1();

        // GET: StudentCourses
        public ActionResult Index()
        {
            var studentCourses = db.StudentCourses.Include(s => s.Course).Include(s => s.Students);
            return View(studentCourses.ToList());
        }


        // GET: StudentCourses/Details/5
        public ActionResult UploadFile()
        {
          
            try
            {
                DataTable t = ExcelHelper.ExcelToDataTable(Request.Files[0].InputStream);
                List<string> columnNames = new List<string>()
                    {
                        "Student Id",
                        "Term Desc Medium",
                        "Course Id",
                        "Course Grade Code"
                    };
                bool containsAllColumns = true;

                foreach (string columnName in columnNames)
                {
                    if (!t.Columns.Contains(columnName))
                    {
                        containsAllColumns = false;
                    }
                }

                if (containsAllColumns)
                {
                  
                    foreach (DataRow b in t.Rows)
                    {
                        var studentId = b["Student Id"].ToString(); //eg. 110118101
                        var termDescMedium = b["Term Desc Medium"].ToString(); //eg. Study Period 2 - 2016
                        var courseId = b["Course Id"].ToString(); //eg. 012398
                        var courseGradeCode = b["Course Grade Code"].ToString(); //eg. P1, HD
                        var courseName = b["Course Desc Long"].ToString(); //eg. ICT Project
                        var courseCode = b["Subject Area Catalog Number"].ToString(); //eg. INFT3025

                        //convert termDescMedium to seperate values, Semester and year
                        var year = "";
                        var semester = "";
                        //termDescMedium needs to be at least x characters long in order
                        //to derive the year and semester
                        if (termDescMedium.Length >= 21)
                        {
                            year = termDescMedium.Substring(termDescMedium.Length - 4);
                            semester = termDescMedium.Substring(termDescMedium.IndexOf("Study Period ") + 13, 1);

                            int semesterParsed;

                            if (int.TryParse(semester, out semesterParsed))
                            {
                                semester = "SP" + semesterParsed;

                                //we good to go
                                //check if there is already an entry
                                using (var db = new Model1())
                                {

                                    var result = (from sc in db.StudentCourses
                                                  join c in db.Course
                                                  on sc.courseID equals c.courseID
                                                  join s in db.Students
                                                  on sc.studentID equals s.studentID
                                                  where s.uniStudentID == studentId && c.courseID == courseId && sc.semester == semester && sc.year.ToString() == year
                                                  select sc)
                                                  .ToList();

                                    if (result.Count != 0)
                                    {
                                        //update
                                        var studentCourse = result.FirstOrDefault();
                                        studentCourse.semester = semester;
                                        //TODO: actually check if these are numbers
                                        studentCourse.year = int.Parse(year);
                                        studentCourse.courseID = courseId;
                                        //TODO: check if grade is null
                                        studentCourse.grade = courseGradeCode;
                                        db.Entry(studentCourse).State = System.Data.Entity.EntityState.Modified;

                                        db.SaveChanges();
                                      

                                    }
                                    else
                                    {
                                        Course course = null;
                                        var courses = (from c in db.Course
                                                       where c.courseID == courseId
                                                       select c)
                                                  .ToList();
                                        if (courses.Count == 0)
                                        {
                                            Course newCourse = new Course
                                            {
                                                courseID = courseId,
                                                courseName = courseName,
                                                courseCode = courseCode,
                                                courseAbbreviation = "-"

                                            };
                                            db.Course.Add(newCourse);
                                            db.SaveChanges();
                                            course = newCourse;

                                        }
                                        else
                                        {
                                            course = courses.FirstOrDefault();

                                        }


                                        var studentCourse = new StudentCourses();

                                        var student = (from s in db.Students
                                                       where s.uniStudentID == studentId
                                                       select s
                                                       )
                                                       .ToList();

                                        if (student.Count != 0)
                                        {
                                            //look up student with the studentId
                                            studentCourse.studentID = student.FirstOrDefault().studentID;
                                            studentCourse.semester = semester;

                                            //TODO: actually check if these are numbers
                                            studentCourse.year = int.Parse(year);
                                            studentCourse.courseID = course.courseID;

                                            //TODO: check if grade is null
                                            studentCourse.grade = courseGradeCode;

                                            var ictm = new Model1();
                                            ictm.StudentCourses.Add(studentCourse);
                                            ictm.SaveChanges();
                                        }
                                      
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (System.ArgumentException a)
            {
                System.Diagnostics.Debug.WriteLine(a.Message);
            }


            return RedirectToAction("Index");
        }


        // GET: StudentCourses/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StudentCourses studentCourses = db.StudentCourses.Find(id);
            if (studentCourses == null)
            {
                return HttpNotFound();
            }
            return View(studentCourses);
        }

        // GET: StudentCourses/Create
        public ActionResult Create()
        {
            ViewBag.courseID = new SelectList(db.Course, "courseID", "courseName");
            ViewBag.studentID = new SelectList(db.Students, "studentID", "uniUserName");
            return View();
        }

        // POST: StudentCourses/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性；有关
        // 更多详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "studentCourseID,studentID,courseID,semester,year,grade,mark")] StudentCourses studentCourses)
        {
            if (ModelState.IsValid)
            {

              var oldd=  db.StudentCourses.FirstOrDefault(p => p.studentID == studentCourses.studentID && p.courseID == studentCourses.courseID);
                if (oldd!=null)
                {
                    oldd.semester = studentCourses.semester;
                    oldd.year = studentCourses.year;
                    oldd.grade = studentCourses.grade;
                    oldd.mark = studentCourses.mark;

                    db.Entry(oldd).State = EntityState.Modified;
                }
                else
                {
                    db.StudentCourses.Add(studentCourses);
                }
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.courseID = new SelectList(db.Course, "courseID", "courseName", studentCourses.courseID);
            ViewBag.studentID = new SelectList(db.Students, "studentID", "uniUserName", studentCourses.studentID);
            return View(studentCourses);
        }

        // GET: StudentCourses/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StudentCourses studentCourses = db.StudentCourses.Find(id);
            if (studentCourses == null)
            {
                return HttpNotFound();
            }
            ViewBag.courseID = new SelectList(db.Course, "courseID", "courseName", studentCourses.courseID);
            ViewBag.studentID = new SelectList(db.Students, "studentID", "uniUserName", studentCourses.studentID);
            return View(studentCourses);
        }

        // POST: StudentCourses/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性；有关
        // 更多详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "studentCourseID,studentID,courseID,semester,year,grade,mark")] StudentCourses studentCourses)
        {
            if (ModelState.IsValid)
            {
                db.Entry(studentCourses).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.courseID = new SelectList(db.Course, "courseID", "courseName", studentCourses.courseID);
            ViewBag.studentID = new SelectList(db.Students, "studentID", "uniUserName", studentCourses.studentID);
            return View(studentCourses);
        }

        // GET: StudentCourses/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StudentCourses studentCourses = db.StudentCourses.Find(id);
            if (studentCourses == null)
            {
                return HttpNotFound();
            }
            return View(studentCourses);
        }

        // POST: StudentCourses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            StudentCourses studentCourses = db.StudentCourses.Find(id);
            db.StudentCourses.Remove(studentCourses);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
