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
    public class ProjectsController : Controller
    {
        private Model1 db = new Model1();

        // GET: Projects
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

            var projects = db.Projects.Include(p => p.ProjectStatus1);
            return View(projects.ToList());
        }


        // GET: Projects
        [HttpPost]
        public ActionResult Index(int ddlyear,string DDLSemester)
        {

            var ddlyearlist=new List<string>();
            var currentDate = System.DateTime.Now;
            for (int i = -2; i <= 2; i++)
            {
                ddlyearlist.Add(currentDate.AddYears(i).Year.ToString());
               
            }
            
            var DDLSemesterlist = new List< string>();
          
            DDLSemesterlist.Add("SP2");
            DDLSemesterlist.Add("SP5");



            ViewBag.ddlyear = new SelectList(ddlyearlist, "");
            ViewBag.DDLSemester = new SelectList(DDLSemesterlist, "");

            var projects = db.Projects.Where(p=> p.projectYear==ddlyear&&p.projectSemester==DDLSemester).Include(p => p.ProjectStatus1);
            return View(projects.ToList());
        }

        // GET: Projects/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Projects projects = db.Projects.Find(id);
            if (projects == null)
            {
                return HttpNotFound();
            }
            return View(projects);
        }

        // GET: Projects/Create
        public ActionResult Create()
        {
            ViewBag.projectStatus = new SelectList(db.ProjectStatus, "ProjectStatusId", "StatusName");
            ViewBag.projectCreatorID = new SelectList(db.AspNetUsers, "personID", "UserName");
            return View();
        }

        // POST: Projects/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性；有关
        // 更多详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "projectID,Id,projectCode,projectTitle,projectScope,projectOutcomes,projectDuration,projectPlacementRequirements,projectSponsorAgreement,projectStatus,projectStatusComment,projectStatusChangeDate,projectSemester,projectSemesterCode,projectYear,projectSequenceNo,honoursUndergrad,requirementsMet,projectCreatorID,dateCreated,projectEffortRequirements,austCitizenOnly,studentsReq,scholarshipAmt,scholarshipDetail,staffEmailSentDate,clientEmailSentDate,studentEmailSentDate")] Projects projects)
        {
         


            if (ModelState.IsValid)
            {
                db.Projects.Add(projects);

                db.SaveChanges();

                if (Request.Files.Count > 0)
                {
                    for (int i = 0; i < Request.Files.Count; i++)
                    {
                        var item = Request.Files[i];


                        if (item.ContentLength>0)
                        {
                            var fjf = $"{Guid.NewGuid().ToString()}.{item.FileName.Split('.').Last().ToString()}";
                            var filename = $"{Server.MapPath("/")}upload/{fjf}";
                          
                            item.SaveAs($"{filename}");
                            Random rd = new Random();

                            var fil = new ProjectDocuments()
                            {
                                projectDocumentID = rd.Next(1, int.MaxValue),
                                documentTitle = item.FileName,
                                documentLink = fjf,
                                documentSource = "client",
                                filePath = fjf,
                                projectID = projects.projectID,
                            };
                            db.ProjectDocuments.Add(fil);
                        }
                     
                    }
                }

                db.SaveChanges();

                return RedirectToAction("Index");
            }

          
            ViewBag.projectCreatorID = new SelectList(db.AspNetUsers, "personID", "UserName");
            ViewBag.projectStatus = new SelectList(db.ProjectStatus, "ProjectStatusId", "StatusName", projects.projectStatus);
            return View(projects);
        }

        // GET: Projects/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Projects projects = db.Projects.Find(id);
            if (projects == null)
            {
                return HttpNotFound();
            }

            ViewBag.projectCreatorID = new SelectList(db.AspNetUsers, "personID", "UserName");

            ViewBag.projectStatus = new SelectList(db.ProjectStatus, "ProjectStatusId", "StatusName", projects.projectStatus);
            return View(projects);
        }

        // POST: Projects/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性；有关
        // 更多详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "projectID,Id,projectCode,projectTitle,projectScope,projectOutcomes,projectDuration,projectPlacementRequirements,projectSponsorAgreement,projectStatus,projectStatusComment,projectStatusChangeDate,projectSemester,projectSemesterCode,projectYear,projectSequenceNo,honoursUndergrad,requirementsMet,projectCreatorID,dateCreated,projectEffortRequirements,austCitizenOnly,studentsReq,scholarshipAmt,scholarshipDetail,staffEmailSentDate,clientEmailSentDate,studentEmailSentDate")] Projects projects)
        {
            if (ModelState.IsValid)
            {
                db.Entry(projects).State = EntityState.Modified;
                db.SaveChanges();

                if (Request.Files.Count > 0)
                {
                    for (int i = 0; i < Request.Files.Count; i++)
                    {
                        var item = Request.Files[i];


                        if (item.ContentLength > 0)
                        {
                            var fjf = $"{Guid.NewGuid().ToString()}.{item.FileName.Split('.').Last().ToString()}";
                            var filename = $"{Server.MapPath("/")}upload/{fjf}";

                            item.SaveAs($"{filename}");
                            Random rd = new Random();

                            var fil = new ProjectDocuments()
                            {
                                projectDocumentID = rd.Next(1, int.MaxValue),
                                documentTitle = item.FileName,
                                documentLink = fjf,
                                documentSource = "client",
                                filePath = fjf,
                                projectID = projects.projectID,
                            };
                            db.ProjectDocuments.Add(fil);
                        }

                    }
                }

                return RedirectToAction("Index");
            }
            ViewBag.projectStatus = new SelectList(db.ProjectStatus, "ProjectStatusId", "StatusName", projects.projectStatus);

            ViewBag.projectCreatorID = new SelectList(db.AspNetUsers, "personID", "UserName");
            return View(projects);
        }

        // GET: Projects/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Projects projects = db.Projects.Find(id);
            if (projects == null)
            {
                return HttpNotFound();
            }
            return View(projects);
        }

        // POST: Projects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
                Projects projects = db.Projects.Find(id);
                db.Projects.Remove(projects);
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
