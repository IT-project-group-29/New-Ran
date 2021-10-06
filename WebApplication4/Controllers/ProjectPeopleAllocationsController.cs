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
    public class ProjectPeopleAllocationsController : Controller
    {
        private Model1 db = new Model1();

        // GET: ProjectPeopleAllocations
        public ActionResult Index()
        {

            var ddlyear = new List<string>();
            var currentDate = System.DateTime.Now;
            for (int i = -2; i <= 2; i++)
            {
                ddlyear.Add(currentDate.AddYears(i).Year.ToString());

            }
            var DDLSemester = new List<string>();

            DDLSemester.Add("SP2");
            DDLSemester.Add("SP5");


            ViewBag.ddlyear = new SelectList(ddlyear, "");
            ViewBag.DDLSemester = new SelectList(DDLSemester, "");

            var projectPeopleAllocations = db.ProjectPeopleAllocations.Include(p => p.Projects);
            ViewBag.personID = db.AspNetUsers.ToList();
            return View(projectPeopleAllocations.ToList());
        }
        [HttpPost]
        public ActionResult Index(int ddlyear, string DDLSemester)
        {

            var ddlyearlist = new List<string>();
            var currentDate = System.DateTime.Now;
            for (int i = -2; i <= 2; i++)
            {
                ddlyearlist.Add(currentDate.AddYears(i).Year.ToString());

            }
            var DDLSemesterlist = new List<string>();

            DDLSemesterlist.Add("SP2");
            DDLSemesterlist.Add("SP5");


            ViewBag.ddlyear = new SelectList(ddlyearlist, "");
            ViewBag.DDLSemester = new SelectList(DDLSemesterlist, "");
 
            var projects = db.Projects.Where(p => p.projectYear == ddlyear && p.projectSemester == DDLSemester).Select(p=>p.projectID).ToList();

            var projectPeopleAllocations = db.ProjectPeopleAllocations.Where(p => projects.Contains(p.projectID)).Include(p => p.Projects);

            ViewBag.personID = db.AspNetUsers.ToList();
            return View(projectPeopleAllocations.ToList());
        }

        // GET: ProjectPeopleAllocations/Details/5
        public ActionResult Details(int projectID,int personID)
        {
           
            ProjectPeopleAllocations projectPeopleAllocations = db.ProjectPeopleAllocations.FirstOrDefault(p=>p.projectID==projectID&&p.personID==personID);

            projectPeopleAllocations.Projects= db.Projects.FirstOrDefault(p => p.projectID == projectID);

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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "projectID,personID,personRole,dateCreated,creatorID,creatorComment")] ProjectPeopleAllocations projectPeopleAllocations)
        {
            var hhh =db.AspNetUsers.FirstOrDefault(p=>p.UserName==User.Identity.Name);
            projectPeopleAllocations.creatorID = hhh.personID;
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
            ViewBag.personID = new SelectList(db.AspNetUsers, "personID", "UserName",projectPeopleAllocations.personID);
            return View(projectPeopleAllocations);
        }

        // POST: ProjectPeopleAllocations/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性；有关
        // 更多详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
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
            ProjectPeopleAllocations projectPeopleAllocations = db.ProjectPeopleAllocations.FirstOrDefault(p=>p.personID==personID&&p.projectID==projectID);
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
}
