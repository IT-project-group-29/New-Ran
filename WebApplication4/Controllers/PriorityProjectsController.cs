using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplication4.Models;

namespace WebApplication4.Controllers
{
    public class PriorityProjectsController : Controller
    {
        private Model1 db = new Model1();

        // GET: PriorityProjects
        public ActionResult Index()
        {

            var ddlyearlist = new List<string>();
            var currentDate = System.DateTime.Now;
            for (int i = -2; i <= 2; i++)
            {
                ddlyearlist.Add(currentDate.AddYears(i).Year.ToString());

            }
            var DDLSemester = new List<string>();
            int yearNow = DateTime.Now.Year;
            int selectyear = 0;
            for (int i = 0; i < ddlyearlist.Count - 1; i++)
            {
                if (Convert.ToInt32(ddlyearlist[i]) == yearNow)
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


            ViewBag.ddlyear = new SelectList(ddlyearlist, ddlyearlist[selectyear]);
            ViewBag.DDLSemester = new SelectList(DDLSemester, DDLSemester[selectMonth]);



            var priorityProjects = db.PriorityProjects.Include(p => p.Projects);
            return View(priorityProjects.ToList());
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

            var projects = db.Projects.Where(p => p.projectYear == ddlyear && p.projectSemester == DDLSemester).Select(p => p.projectID).ToList();

            var priorityProjects = db.PriorityProjects.Where(p=>projects.Contains(p.projectID)).Include(p => p.Projects);
            return View(priorityProjects.ToList());
        }

        // GET: PriorityProjects/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
          
            PriorityProjects priorityProjects = db.PriorityProjects.FirstOrDefault(p=>p.projectID==id.Value);
            priorityProjects.Projects= db.Projects.FirstOrDefault(p => p.projectID == priorityProjects.projectID);
            if (priorityProjects == null)
            {
                return HttpNotFound();
            }
            return View(priorityProjects);
        }

        // GET: PriorityProjects/Create
        public ActionResult Create()
        {
            var pids=db.PriorityProjects.Select(p => p.projectID).ToList();
            var plist= db.Projects.Where(p => !pids.Contains(p.projectID)).ToList();
            ViewBag.projectID = new SelectList(plist, "projectID", "projectTitle");
            return View();
        }

        // POST: PriorityProjects/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性；有关
        // 更多详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "projectID,dateCreated,priorityLevel,priorityReason,priorityCreatorID")] PriorityProjects priorityProjects)
        {
            DateTime dttn = DateTime.Now;

            string format = DateTime.Now.ToString();
            dttn = Convert.ToDateTime(format);
            
            
            priorityProjects.dateCreated = dttn;
            
            if (ModelState.IsValid)
            {
                try
                {
                   
                    db.PriorityProjects.Add(priorityProjects);
                    db.SaveChanges();
                }
                catch (Exception)
                {

                    throw;
                }
            
                return RedirectToAction("Index");
            }

            ViewBag.projectID = new SelectList(db.Projects, "projectID", "Id", priorityProjects.projectID);
            return View(priorityProjects);
        }

        // GET: PriorityProjects/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);//An error request is returned. The ID cannot be empty
            }
            PriorityProjects priorityProjects = db.PriorityProjects.FirstOrDefault(p=>p.projectID==id.Value);//Create an initial database
            if (priorityProjects == null)
            {
                return HttpNotFound();//If the ID is not found, a web page error is returned
            }
            ViewBag.projectID = new SelectList(db.Projects, "projectID", "Id", priorityProjects.projectID);//Initializes a new instance of the SelectList class with the specified item and selected value of the list.
            return View(priorityProjects);
        }

        // POST: PriorityProjects/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性；有关
        // 更多详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "projectID,dateCreated,priorityLevel,priorityReason,priorityCreatorID")] PriorityProjects priorityProjects)
        {
            if (ModelState.IsValid)
            {
                var data= db.PriorityProjects.FirstOrDefault(p => p.projectID == priorityProjects.projectID);//Create an initial database
                data.priorityLevel = priorityProjects.priorityLevel;
                data.priorityReason= priorityProjects.priorityReason;
                db.Entry(data).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.projectID = new SelectList(db.Projects, "projectID", "Id", priorityProjects.projectID); //Initializes a new instance of the SelectList class with the specified item and selected value of the list.
            return View(priorityProjects);
        }

        // GET: PriorityProjects/Delete/5
        public ActionResult Delete(int? id , DateTime dt)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PriorityProjects priorityProjects = db.PriorityProjects.FirstOrDefault(p=>p.projectID==id.Value);
            priorityProjects.Projects = db.Projects.FirstOrDefault(p => p.projectID == priorityProjects.projectID);
            if (priorityProjects == null)
            {
                return HttpNotFound();
            }
            return View(priorityProjects);
        }

        //POST: PriorityProjects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id,DateTime dt)
        {
            
            
           

         
                PriorityProjects priorityProjects = db.PriorityProjects.FirstOrDefault(p => p.projectID == id);
            
                if(priorityProjects.dateCreated == dt)
            {
                db.PriorityProjects.Remove(priorityProjects);

                db.SaveChanges();
            }
                
            

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
