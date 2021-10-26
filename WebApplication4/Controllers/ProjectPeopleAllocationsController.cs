﻿using System;
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
    public class ProjectPeopleAllocationsController : Controller
    {
        private Model1 db = new Model1();

        // GET: ProjectPeopleAllocations
        public ActionResult Index()
        {
            List<StudentInfo> StdInfom = GetStudents();
        


    
            ViewBag.namedesc = "asc";
            //ViewBag.stcs = db.StudentCourses.ToList();
            ViewBag.stcs = StdInfom;
            var ddlyear = new List<string>();
            var currentDate = System.DateTime.Now;


            for (int i = -2; i <= 2; i++)
            {
                ddlyear.Add(currentDate.AddYears(i).Year.ToString());

            }
            var DDLSemester = new List<string>();
            int yearNow = DateTime.Now.Year;
            int selectyear = 0;
            for (int i = 0; i < ddlyear.Count - 1; i++)
            {
                if (Convert.ToInt32(ddlyear[i]) == yearNow)
                {
                    selectyear = i;
                }
            }
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
            ViewBag.pop = "student";
            ViewBag.ddlyear = new SelectList(ddlyear, ddlyear[selectyear]);
            ViewBag.DDLSemester = new SelectList(DDLSemester,DDLSemester[selectMonth]);
            ViewBag.staff = db.Staff.ToList();
            ViewBag.staffHidden = "hidden";
            var projectPeopleAllocations = db.ProjectPeopleAllocations.Include(p => p.Projects);
            ViewBag.personID = db.AspNetUsers.ToList();
            return View(projectPeopleAllocations.ToList());
        }
        [HttpPost]
        public ActionResult Index(int ddlyear, string DDLSemester, string namedesc, string pop,string tyt )
        {
            List<StudentInfo> StdInfom = GetStudents();
            ViewBag.tyt = tyt;
            ViewBag.stcs = StdInfom;
            ViewBag.pop = pop;
            ViewBag.staff = db.Staff.ToList();
            if (pop != "student")
                {
                    ViewBag.staffHidden = "";
                    ViewBag.studenthidden = "hidden";
                }
                else
                {
                    ViewBag.staffHidden = "hidden";
                    ViewBag.studenthidden = "";
                }
            
            
            var ddlyearlist = new List<string>();
            var currentDate = System.DateTime.Now;
            for (int i = -2; i <= 2; i++)
            {
                ddlyearlist.Add(currentDate.AddYears(i).Year.ToString());

            }
            var DDLSemesterlist = new List<string>();

            List<SelectListItem> DDLSEM = new List<SelectListItem>();
            DDLSEM.Add(new SelectListItem { Text = "SP2" });
            DDLSEM.Add(new SelectListItem { Text = "SP5" ,Selected = true});
            DDLSemesterlist.Add("SP2");
            DDLSemesterlist.Add("SP5");


           


            if (namedesc == "asc" )
            {
                if (tyt == "name")
                {

                    ViewBag.stcs = StdInfom.OrderBy(p => p.Name);
                    ViewBag.staff = db.Staff.OrderBy(p => p.username).ToList();
                }
                if(tyt == "gpa")
                {

                    ViewBag.stcs = StdInfom.OrderByDescending(p => p.Gpa);
                    
                }
                if (tyt == "105295")
                {
                    
                    ViewBag.stcs = StdInfom.OrderBy(p => p.CPP);
                    
                }
                if (tyt == "105294")
                {

                    ViewBag.stcs = StdInfom.OrderBy(p => p.PF);
                    
                }
                if (tyt == "156783")
                {

                    ViewBag.stcs = StdInfom.OrderBy(p => p.WEB);
                    
                }
                if (tyt == "012540")
                {

                    ViewBag.stcs = StdInfom.OrderBy(p => p.IDIE);
                    
                }
                if (tyt == "105284")
                {

                    ViewBag.stcs = StdInfom.OrderBy(p => p.AgNET);
                    
                }




            }
            if (namedesc == "desc")
            {
                if (tyt == "name")
                {
                    ViewBag.stcs = StdInfom.OrderByDescending(p => p.Name);
                    ViewBag.staff = db.Staff.OrderByDescending(p => p.username).ToList();
                }
                if (tyt == "gpa")
                {
                    ViewBag.stcs = StdInfom.OrderBy(p => p.Gpa);
               
                }
                if (tyt == "105295")
                {

                    ViewBag.stcs = StdInfom.OrderByDescending(p => p.CPP);
                    
                }
                if (tyt == "105294")
                {

                    ViewBag.stcs = StdInfom.OrderByDescending(p => p.PF);
                  
                }
                if (tyt == "156783")
                {

                    ViewBag.stcs = StdInfom.OrderByDescending(p => p.WEB);
                   
                }
                if (tyt == "012540")
                {

                    ViewBag.stcs = StdInfom.OrderByDescending(p => p.IDIE);
                   
                }
                if (tyt == "105284")
                {

                    ViewBag.stcs = StdInfom.OrderByDescending(p => p.AgNET);
                   
                }


            }


        



            ViewBag.ddlyear = new SelectList(ddlyearlist, "");
            ViewBag.DDLSemester = new SelectList(DDLSemesterlist, "");

            var projects = db.Projects.Where(p => p.projectYear == ddlyear && p.projectSemester == DDLSemester).Select(p => p.projectID).ToList();

            var projectPeopleAllocations = db.ProjectPeopleAllocations.Where(p => projects.Contains(p.projectID)).Include(p => p.Projects);

            ViewBag.personID = db.AspNetUsers.ToList();
            return View(projectPeopleAllocations.ToList());
        }

        // GET: ProjectPeopleAllocations/Details/5
        public ActionResult Details(int projectID, int personID)
        {

            ProjectPeopleAllocations projectPeopleAllocations = db.ProjectPeopleAllocations.FirstOrDefault(p => p.projectID == projectID && p.personID == personID);

            projectPeopleAllocations.Projects = db.Projects.FirstOrDefault(p => p.projectID == projectID);

            projectPeopleAllocations.Users = db.AspNetUsers.FirstOrDefault(p => p.personID == personID);
            if (projectPeopleAllocations == null)
            {
                return HttpNotFound();
            }
            return View(projectPeopleAllocations);
        }

        // GET: ProjectPeopleAllocations/Create
        public ActionResult Create()
        {
            ViewBag.projectID = new SelectList(db.Projects, "projectID", "projectTitle");
            ViewBag.personID = new SelectList(db.AspNetUsers, "personID", "UserName");
            return View();
        }

        // POST: ProjectPeopleAllocations/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性；有关
        // 更多详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        private List<StudentInfo> GetStudents()
        {
            List<StudentInfo> StdInfom = new List<StudentInfo>();
            foreach (var item in db.Students)
            {
                
                StdInfom.Add(new StudentInfo
                {
                    ID = item.studentID,
                    Name = item.uniUserName,
                    Gpa = item.gpa,
                    CPP = db.StudentCourses.FirstOrDefault(p => p.studentID == item.studentID && p.courseID == "105295").grade,
                    PF = db.StudentCourses.FirstOrDefault(p => p.studentID == item.studentID && p.courseID == "105294").grade,
                    WEB = db.StudentCourses.FirstOrDefault(p => p.studentID == item.studentID && p.courseID == "156783").grade,
                    IDIE = db.StudentCourses.FirstOrDefault(p => p.studentID == item.studentID && p.courseID == "012540").grade,
                    AgNET = db.StudentCourses.FirstOrDefault(p => p.studentID == item.studentID && p.courseID == "105284").grade
                });


               
            }
            return StdInfom;
        }
    
 

    [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "projectID,personID,personRole,dateCreated,creatorID,creatorComment")] ProjectPeopleAllocations projectPeopleAllocations)
        {
            var hhh = db.AspNetUsers.FirstOrDefault(p => p.UserName == User.Identity.Name);
            projectPeopleAllocations.creatorID = hhh.personID;

            projectPeopleAllocations.dateCreated = DateTime.Now;
            if (ModelState.IsValid)
            {

                db.ProjectPeopleAllocations.Add(projectPeopleAllocations);
                db.SaveChanges();


                return RedirectToAction("Index");
            }

            ViewBag.projectID = new SelectList(db.Projects, "projectID", "Id", projectPeopleAllocations.projectID);
            return View(projectPeopleAllocations);
        }

        // GET: ProjectPeopleAllocations/Edit/5
        public ActionResult Edit(int projectID, int personID)
        {

            ProjectPeopleAllocations projectPeopleAllocations = db.ProjectPeopleAllocations.FirstOrDefault(p => p.projectID == projectID && p.personID == personID);
            if (projectPeopleAllocations == null)
            {
                return HttpNotFound();
            }
            ViewBag.projectID = new SelectList(db.Projects, "projectID", "Id", projectPeopleAllocations.projectID);
            ViewBag.personID = new SelectList(db.AspNetUsers, "personID", "UserName", projectPeopleAllocations.personID);
            return View(projectPeopleAllocations);
        }

        // POST: ProjectPeopleAllocations/Edit/5
      
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "projectID,personID,personRole,dateCreated,creatorID,creatorComment")] ProjectPeopleAllocations projectPeopleAllocations)
        {
            if (ModelState.IsValid)
            {
                db.Entry(projectPeopleAllocations).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.projectID = new SelectList(db.Projects, "projectID", "Id", projectPeopleAllocations.projectID);
            ViewBag.personID = new SelectList(db.AspNetUsers, "personID", "UserName", projectPeopleAllocations.personID);
            return View(projectPeopleAllocations);
        }

        // GET: ProjectPeopleAllocations/Delete/5
        public ActionResult Delete(int projectID, int personID)
        {

            ProjectPeopleAllocations projectPeopleAllocations = db.ProjectPeopleAllocations.FirstOrDefault(p => p.projectID == projectID && p.personID == personID);

            projectPeopleAllocations.Projects = db.Projects.FirstOrDefault(p => p.projectID == projectID);
            projectPeopleAllocations.Users = db.AspNetUsers.FirstOrDefault(p => p.personID == personID);

            if (projectPeopleAllocations == null)
            {
                return HttpNotFound();
            }
            return View(projectPeopleAllocations);
        }

        // POST: ProjectPeopleAllocations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int projectID, int personID)
        {
            ProjectPeopleAllocations projectPeopleAllocations = db.ProjectPeopleAllocations.FirstOrDefault(p => p.personID == personID && p.projectID == projectID);
            db.ProjectPeopleAllocations.Remove(projectPeopleAllocations);
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
    public class StudentInfo
    {
        public int ID { set; get; }
        public string Name { set; get; }
        public decimal? Gpa { set; get; }
        public string CPP { set; get; }

        public string PF { set; get; }
        public string WEB { set; get; }
        public string IDIE { set; get; }
        public string AgNET { set; get; }

    }
}
