using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Web;
using System.Web.Mvc;
using WebApplication4.Models;

namespace WebApplication4.Controllers
{
    public class EmailController : Controller
    {
        // GET: Email
        public ActionResult LiEmailStudents()
        {
           
            return View();
        }
        [HttpPost]
        public ActionResult LiEmailStudents(string email, string subject, string body)
        {
            sendEmail(subject: subject, body: body);
            return View();
        }
        // GET: Email
        public ActionResult LiEmailStaff()
        {
          
            return View();
        }
        [HttpPost]
        public ActionResult LiEmailStaff(string email, string subject, string body)
        {
            sendEmail(subject: subject, body: body);
            return View();
        }
        public ActionResult LiEmailClients()
        {
          
            return View();
        }
        [HttpPost]
        public ActionResult LiEmailClients(string email, string subject, string body)
        {
            sendEmail(subject: subject, body: body);
            return View();
        }

        // TODO: Use Email Helper function in Helpers folder
        public void sendEmail(string[] to = null, string subject = "", string body = "", bool isBodyHtml = false, ProjectDocuments[] projectDocuments = null)
        {
            MailAddress david = new MailAddress("David.Harris@unisa.edu.au", "David Harris");
            MailAddress stemICTProject = new MailAddress("**yourTestEmailAcct.com", "displayName");

            using (MailMessage mail = new MailMessage())
            {
                mail.From = new MailAddress("**YourOwnGmailAccount.com**", "UniSA ICT Projects");
                mail.ReplyToList.Add(stemICTProject);
                mail.CC.Add(stemICTProject);
                //mail.CC.Add(nicole);
                mail.Priority = MailPriority.High;

                foreach (var t in to)
                {
                    mail.To.Add(t);
                }
                mail.Subject = subject;
                mail.Body = body;


                mail.IsBodyHtml = isBodyHtml;
                if (projectDocuments != null)
                {
                    foreach (ProjectDocuments pDoc in projectDocuments)
                    {
                        string fileDir = HttpContext.Server.MapPath(pDoc.filePath);

                        Attachment attch = new Attachment(fileDir, MediaTypeNames.Application.Octet);

                        attch.ContentDisposition.CreationDate = System.IO.File.GetCreationTime(fileDir);
                        attch.ContentDisposition.ModificationDate = System.IO.File.GetLastWriteTime(fileDir);
                        attch.ContentDisposition.ReadDate = System.IO.File.GetLastAccessTime(fileDir);

                        mail.Attachments.Add(attch);
                    }
                }

                using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                {
                    smtp.Credentials = new NetworkCredential("**YourOwnGmailAccount.com**", "yourPassword*");
                    smtp.EnableSsl = true;
                    smtp.Send(mail);
                }
            }
        }
    }
}