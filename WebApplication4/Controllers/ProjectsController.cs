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
            //create an empty List for ddlyear and currentDate is used to get the current date now
            var ddlyear = new List<string>();
            var currentDate = System.DateTime.Now;

            //because we will show 5 years, so we'll get first and second two years of this year, 
            //so use i = -2 to i <=2
            for (int i = -2; i <= 2; i++)
            {
                ddlyear.Add(currentDate.AddYears(i).Year.ToString());
            }

            //create an empty list for DDLSemester and get the year now stored in yearNow
            var DDLSemester = new List<string>();
            int yearNow = DateTime.Now.Year;
            int selectyear = 0;
            //check if the ddlyear is equal to yearNow
            for (int i = 0; i < ddlyear.Count - 1; i++)
            {
                if (Convert.ToInt32(ddlyear[i]) == yearNow)
                {
                    selectyear = i;
                }
            }
            //add SP2 and SP5 to DDLSemester list
            DDLSemester.Add("SP2");
            DDLSemester.Add("SP5");
            //check if now.Month<6 the semester is SP2 else it is SP5
            int selectMonth = 0;
            if (DateTime.Now.Month < 6)
            {
                selectMonth = 0;
            }
            else
            {
                selectMonth = 1;
            }

            //store the year and semester now in the ViewBage of ddlyear and semester
            ViewBag.ddlyear = new SelectList(ddlyear, ddlyear[selectyear]);
            ViewBag.DDLSemester = new SelectList(DDLSemester, DDLSemester[selectMonth]);
            //put the year and semester to ViewBag.project if they are equal to the true year or semester
            var myeey = Convert.ToInt32(ddlyear[selectyear]);
            var sem = DDLSemester[selectMonth];
            ViewBag.project = db.Projects.Where(p => p.projectYear == myeey && p.projectSemester == sem).ToList();

            var projects = db.Projects.Include(p => p.ProjectStatus1);
            return View(projects.ToList());
        }


        // GET: Projects
        [HttpPost]
        public ActionResult Index(int ddlyear, string DDLSemester)
        {
            ViewBag.project = db.Projects.Where(p => p.projectYear == ddlyear && p.projectSemester == DDLSemester).ToList();

            var ddlyearlist = new List<string>();
            var currentDate = System.DateTime.Now;
            //add five year to ddlyearlist
            for (int i = -2; i <= 2; i++)
            {
                ddlyearlist.Add(currentDate.AddYears(i).Year.ToString());
            }

            var DDLSemesterlist = new List<string>();
            //List<SelectListItem> DDLSEM = new List<SelectListItem>();
            //DDLSEM.Add(new SelectListItem { Text = "SP2" });
            //DDLSEM.Add(new SelectListItem { Text = "SP5", Selected = true });
            DDLSemesterlist.Add("SP2");
            DDLSemesterlist.Add("SP5");

            ViewBag.ddlyear = new SelectList(ddlyearlist, "");
            ViewBag.DDLSemester = new SelectList(DDLSemesterlist, "");

            var projects = db.Projects.Where(p => p.projectYear == ddlyear && p.projectSemester == DDLSemester).Include(p => p.ProjectStatus1);
            return View(projects.ToList());
        }

        // GET: Projects/Details/5
        public ActionResult Details(int? id)
        {
            //if id or projects is null, mark http response status as 404
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
            //the function for create page
            ViewBag.projectStatus = new SelectList(db.ProjectStatus, "ProjectStatusId", "StatusName");
            ViewBag.projectCreatorID = new SelectList(db.AspNetUsers, "personID", "UserName");
            return View();
        }

        // POST: Projects/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,projectID,projectCode,projectTitle,projectScope,projectOutcomes,projectDuration,projectPlacementRequirements,projectSponsorAgreement,projectStatus,projectStatusComment,projectStatusChangeDate,projectSemester,projectSemesterCode,projectYear,projectSequenceNo,honoursUndergrad,requirementsMet,projectCreatorID,dateCreated,projectEffortRequirements,austCitizenOnly,studentsReq,scholarshipAmt,scholarshipDetail,staffEmailSentDate,clientEmailSentDate,studentEmailSentDate")] Projects projects)
        {
            //select all the information the need to input when create
            if (ModelState.IsValid)
            {
                db.Projects.Add(projects);

                db.SaveChanges();

                if (Request.Files.Count > 0)
                {
                    for (int i = 0; i < Request.Files.Count; i++)
                    {
                        var item = Request.Files[i];
                        //this is the uplod file function
                        if (item.ContentLength > 0)
                        {
                            //get the path and the name of the file
                            var fjf = $"{Guid.NewGuid().ToString()}.{item.FileName.Split('.').Last().ToString()}";
                            var filename = $"{Server.MapPath("/")}upload/{fjf}";

                            item.SaveAs($"{filename}");
                            Random rd = new Random();
                            //as the file which uploaded, add the information to database from the file
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


            ViewBag.projectCreatorID = new SelectList(db.AspNetUsers, "personID", "UserName");
            ViewBag.projectStatus = new SelectList(db.ProjectStatus, "ProjectStatusId", "StatusName", projects.projectStatus);
            return View(projects);
        }

        // GET: Projects/Edit/5
        public ActionResult Edit(int? id)
        { 
            //if id or projects is null, mark http response status as 404
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);//An error request is returned. The ID cannot be empty
            }
            Projects projects = db.Projects.Find(id);//Create a database
            if (projects == null)
            {
                return HttpNotFound();//If the ID is not found, a web page error is returned
            }

            ViewBag.projectCreatorID = new SelectList(db.AspNetUsers, "personID", "UserName");//Initializes a new instance of the SelectList class with the specified item and selected value of the list.

            ViewBag.projectStatus = new SelectList(db.ProjectStatus, "ProjectStatusId", "StatusName", projects.projectStatus);//Initializes a new instance of the SelectList class with the specified item and selected value of the list.
            return View(projects);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,projectID,projectCode,projectTitle,projectScope,projectOutcomes,projectDuration,projectPlacementRequirements,projectSponsorAgreement,projectStatus,projectStatusComment,projectStatusChangeDate,projectSemester,projectSemesterCode,projectYear,projectSequenceNo,honoursUndergrad,requirementsMet,projectCreatorID,dateCreated,projectEffortRequirements,austCitizenOnly,studentsReq,scholarshipAmt,scholarshipDetail,staffEmailSentDate,clientEmailSentDate,studentEmailSentDate")] Projects projects)
        {
            if (ModelState.IsValid)
            {
                db.Entry(projects).State = EntityState.Modified;//If the status of the entity is equal to the changed value
                db.SaveChanges();

                if (Request.Files.Count > 0)
                {
                    for (int i = 0; i < Request.Files.Count; i++) //Make a loop.
                    {
                        var item = Request.Files[i];


                        if (item.ContentLength > 0)
                        {
                            //get the path and the name of the file
                            var fjf = $"{Guid.NewGuid().ToString()}.{item.FileName.Split('.').Last().ToString()}";
                            var filename = $"{Server.MapPath("/")}upload/{fjf}";

                            item.SaveAs($"{filename}");
                            Random rd = new Random();
                            //as the file which uploaded, add the information to database from the file
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
            ViewBag.projectStatus = new SelectList(db.ProjectStatus, "ProjectStatusId", "StatusName", projects.projectStatus);//Initializes a new instance of the SelectList class with the specified item and selected value of the list.

            ViewBag.projectCreatorID = new SelectList(db.AspNetUsers, "personID", "UserName");//Initializes a new instance of the SelectList class with the specified item and selected value of the list.
            return View(projects);
        }

        // GET: Projects/Delete/5
        public ActionResult Delete(int? id)
        {
            //if id or projects is null, mark http response status as 404
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
