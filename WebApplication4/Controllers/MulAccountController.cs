using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebApplication4.Models;
using WebApplication4.Util;

namespace WebApplication4.Controllers
{
    public class MulAccountController : Controller
    {
        private Model1 db = new Model1();

        // GET: Students
        public ActionResult Index()
        {
            return View();
        }

      

        // GET: Students/Create
        public async Task Create(int year,string Semester)
        {
            try
            {
                //Request.Files[0].InputStream;
                DataTable t = ExcelHelper.ExcelToDataTable(Request.Files[0].InputStream);
                //clone copies the structure but not the rows
                DataTable succesfulInserts = t.Clone();
                DataTable failedInserts = t.Clone();
                DataTable updatedInserts = t.Clone();
                failedInserts.Columns.Add("Reason", typeof(string));
                updatedInserts.Columns.Add("Columns Changed", typeof(string));
                failedInserts.Columns["Reason"].SetOrdinal(0);
                updatedInserts.Columns["Columns Changed"].SetOrdinal(0);

                //fix GPA decimal places
                if (t.Columns.Contains("Current GPA"))
                {
                    foreach (DataRow b in t.Rows)
                    {
                        try
                        {
                            //TODO: validate the uniusername
                            string studentUsername;
                            if (b["Student Email Address"].ToString().Replace("@mymail.unisa.edu.au", "").Length > 12)
                            {
                                studentUsername = b["Student Email Address"].ToString().Replace("@mymail.unisa.edu.au", "").Substring(0, 12);
                            }
                            else
                            {
                                studentUsername = b["Student Email Address"].ToString().Replace("@mymail.unisa.edu.au", "");
                            }

                            //this fixed the floating point issue of GPA's
                            b["Current GPA"] = Math.Round(double.Parse(b["Current GPA"].ToString()), 2);


                            //first need to create an AspNetUser
                            var manager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
                            //manager.FindByName(b["Student Email Address"].ToString()));
                            #region insert student
                            //if there is no current account with the given email
                            if (null ==await manager.FindByNameAsync(studentUsername))
                            {
                                var user = new ApplicationUser()
                                {
                                    UserName = studentUsername,
                                    Email = b["Student Email Address"].ToString(),
                                    firstName = b["Student First Name"].ToString(),
                                    lastName = b["Student Family Name"].ToString(),
                                    dateCreated = System.DateTime.Now
                                };
                                try
                                {
                                    IdentityResult result = manager.Create(user, (studentUsername + b["Student Id"].ToString()));


                                    if (result.Succeeded)
                                    {
                                        var student = new Students();

                                        using (var db = new Model1())
                                        {
                                            var s = db.AspNetUsers.Find(user.Id);
                                            student.studentID = s.personID;



                                            student.uniUserName = studentUsername;
                                            student.studentEmail = user.Email;
                                            student.uniStudentID = b["Student Id"].ToString();
                                            student.genderCode = Convert.ToString(b["Gender Code"]);
                                            student.international = Convert.ToString(b["International Student Flag"]);
                                            student.gpa = decimal.Parse(b["Current GPA"].ToString());
                                            student.externalStudent = "Off-Site Location".Equals(Convert.ToString(b["Location Desc Medium"]).Trim());

                                            student.year = year;
                                            student.semester = Semester;

                                            var plan = Convert.ToString(b["Program Plan"]);
                                            var planId = (from a in db.Plans
                                                          where a.planCode == plan
                                                          select a.planId).ToArray().FirstOrDefault();

                                            student.planId = planId;


                                            var studentComment = new PeopleComments();

                                            studentComment.comment = "Successfully added from Excel";
                                            studentComment.commentPersonID = db.AspNetUsers.Find(User.Identity.GetUserId()).personID;
                                            studentComment.personID = student.studentID;
                                            studentComment.commentDate = DateTime.Now;


                                            var ictm = new Model1();
                                            ictm.Students.Add(student);
                                            ictm.PeopleComments.Add(studentComment);
                                            ictm.SaveChanges();
                                        }


                                        succesfulInserts.ImportRow(b);

                                        //adds users to the Student role
                                        UserRoleHelper.addUserToRole(user.Id, "Student");


                                    }
                                    else
                                    {
                                        failedInserts.ImportRow(b);
                                        failedInserts.Rows[failedInserts.Rows.Count - 1]["Reason"] = result.Errors.FirstOrDefault();
                                    }
                                }
                                catch (Exception ex)
                                {
                                    failedInserts.ImportRow(b);
                                    failedInserts.Rows[failedInserts.Rows.Count - 1]["Reason"] = ex.Message;
                                    if (ex is DbEntityValidationException)
                                    {
                                        var e2 = ex as DbEntityValidationException;
                                        foreach (var eve in e2.EntityValidationErrors)
                                        {
                                            var s1 = eve.Entry.Entity.GetType().Name;
                                            string s2 = "";
                                            foreach (var ve in eve.ValidationErrors)
                                            {
                                                s2 = ve.PropertyName + " - " + ve.ErrorMessage;
                                            }
                                            failedInserts.Rows[failedInserts.Rows.Count - 1]["Reason"] += Environment.NewLine + s1 + Environment.NewLine + s2;
                                        }
                                    }
                                }
                            }
                            #endregion
                            #region update student
                            //if there is an account already with the given email
                            else
                            {
                                string columnsChanged = "";
                                ApplicationUser user = manager.FindByName(studentUsername);

                                //if statments to check each attribute of the DB.AspNetUsers
                                if (user.firstName != b["Student First Name"].ToString())
                                {
                                    user.firstName = b["Student First Name"].ToString();
                                    columnsChanged += "Student First Name, ";
                                }
                                if (user.lastName != b["Student Family Name"].ToString())
                                {
                                    user.lastName = b["Student Family Name"].ToString();
                                    columnsChanged += "Student Family Name, ";
                                }


                                manager.Update(user);

                                int userPersonID;
                                using (var db = new Model1())
                                {
                                    var s = db.AspNetUsers.Find(user.Id);
                                    userPersonID = s.personID;

                                    var result = db.Students.Where(stud => stud.studentID == userPersonID);

                                    var studentRecord = result.FirstOrDefault();


                                    if (null != studentRecord)
                                    {
                                        //if statements to check each attribute of DB.Students
                                        if (studentRecord.uniStudentID != b["Student Id"].ToString())
                                        {
                                            studentRecord.uniStudentID = b["Student Id"].ToString();
                                            columnsChanged += "Uni Student ID, ";
                                        }

                                        if (studentRecord.gpa != decimal.Parse(b["Current GPA"].ToString()))
                                        {
                                            studentRecord.gpa = decimal.Parse(b["Current GPA"].ToString());
                                            columnsChanged += "Current GPA, ";
                                        }

                                        if (studentRecord.genderCode != Convert.ToString(b["Gender Code"]))
                                        {
                                            studentRecord.genderCode = Convert.ToString(b["Gender Code"]);
                                        }

                                        if (studentRecord.international != Convert.ToString(b["International Student Flag"]))
                                        {
                                            studentRecord.international = Convert.ToString(b["International Student Flag"]);
                                        }

                                        if (studentRecord.studentEmail != Convert.ToString(b["Student Email Address"]))
                                        {
                                            studentRecord.studentEmail = Convert.ToString(b["Student Email Address"]);
                                        }

                                        studentRecord.externalStudent = "Off-Site Location".Equals(Convert.ToString(b["Location Desc Medium"]).Trim());


                                        var planFromFile = b["Program Plan"].ToString();

                                        int planId = (from a in db.Plans
                                                      where a.planCode == planFromFile
                                                      select a.planId).ToArray().FirstOrDefault();

                                        if (planId == 0)
                                        {
                                            //TODO:
                                            //check that the "no-plan" plan exists in the database
                                            planId = (from a in db.Plans
                                                      where a.planName == "No plan"
                                                      select a.planId).ToArray().FirstOrDefault();

                                        }

                                        if (studentRecord.planId != planId)
                                        {
                                            studentRecord.planId = planId;
                                            columnsChanged += "Plan Id, ";
                                        }
                                        if (studentRecord.year != year)
                                        {
                                            studentRecord.year = year;
                                            columnsChanged += "Year, ";
                                        }
                                        if (studentRecord.semester != Semester)
                                        {
                                            studentRecord.semester = Semester;
                                            columnsChanged += "Semester, ";
                                        }

                                    }


                                    db.SaveChanges();
                                    //if no columns have changed
                                    if ("" == columnsChanged)
                                    {

                                        var studentComment = new PeopleComments();

                                        studentComment.comment = "Duplicate excel upload attempt";
                                        studentComment.commentPersonID = db.AspNetUsers.Find(User.Identity.GetUserId()).personID;
                                        studentComment.personID = userPersonID;
                                        studentComment.commentDate = DateTime.Now;

                                        var ictm = new Model1();
                                        ictm.PeopleComments.Add(studentComment);
                                        ictm.SaveChanges();

                                        failedInserts.ImportRow(b);
                                        failedInserts.Rows[failedInserts.Rows.Count - 1]["Reason"] = "Duplicate user in system";
                                    }
                                    //if at least one column has changed
                                    else
                                    {
                                        //trims off the ,
                                        columnsChanged = columnsChanged.Substring(0, columnsChanged.Length - 2);
                                        var studentComment = new PeopleComments();


                                        studentComment.comment = "Successfully updated columns: " + columnsChanged;
                                        studentComment.commentPersonID = db.AspNetUsers.Find(User.Identity.GetUserId()).personID;
                                        studentComment.personID = userPersonID;
                                        studentComment.commentDate = DateTime.Now;

                                        var ictm = new Model1();
                                        ictm.PeopleComments.Add(studentComment);
                                        ictm.SaveChanges();
                                        updatedInserts.ImportRow(b);
                                        updatedInserts.Rows[updatedInserts.Rows.Count - 1]["Columns Changed"] = columnsChanged;
                                    }
                                }



                            }
                            #endregion
                        }
                        catch (Exception ex)
                        {
                            //TODO
                            System.Diagnostics.Debug.WriteLine(ex.StackTrace);

                        }

                    }
                }

                failedInserts.AcceptChanges();
                succesfulInserts.AcceptChanges();
                updatedInserts.AcceptChanges();
            }
            catch (System.ArgumentException a)
            {
                System.Diagnostics.Debug.WriteLine(a.Message);
            }
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
