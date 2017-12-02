using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Web;  

namespace PrivateTutorOnline.Services
{
    public static class EmailSenderService
    {
        public static void SendHtmlFormattedEmail(string recepientEmail, string subject, string body)
        {
            using (MailMessage mailMessage = new MailMessage())
            {
                mailMessage.From = new MailAddress(ConfigurationManager.AppSettings["UserName"]);
                mailMessage.Subject = subject;
                mailMessage.Body = body;
                mailMessage.IsBodyHtml = true;
                mailMessage.To.Add(new MailAddress(recepientEmail));
                SmtpClient smtp = new SmtpClient();
                smtp.Host = ConfigurationManager.AppSettings["Host"];
                smtp.EnableSsl = Convert.ToBoolean(ConfigurationManager.AppSettings["EnableSsl"]);
                System.Net.NetworkCredential NetworkCred = new System.Net.NetworkCredential();
                NetworkCred.UserName = ConfigurationManager.AppSettings["UserName"];
                NetworkCred.Password = ConfigurationManager.AppSettings["Password"];
                smtp.UseDefaultCredentials = true;
                smtp.Credentials = NetworkCred;
                smtp.Port = int.Parse(ConfigurationManager.AppSettings["Port"]);
                smtp.Send(mailMessage);
            }
        } 
        public static string PopulateBody(string FullName, string templateUrl)
        {
            string body = string.Empty;
            using (StreamReader reader = new StreamReader(HttpContext.Current.Server.MapPath(templateUrl)))
            {
                body = reader.ReadToEnd();
            }
            body = body.Replace("[FullName]", FullName); 
            return body;
        }
        public static string PopulateBody(string FullName, string TutorFullName, string ClassCode, string templateUrl)
        {
            string body = string.Empty;
            using (StreamReader reader = new StreamReader(HttpContext.Current.Server.MapPath(templateUrl)))
            {
                body = reader.ReadToEnd();
            }
            body = body.Replace("[FullName]", FullName);
            body = body.Replace("[TutorFullName]", TutorFullName);
            body = body.Replace("[ClassCode]", ClassCode);
            return body;
        }
        public static string PopulateBody(string FullName, string Username, string templateUrl)
        {
            string body = string.Empty;
            using (StreamReader reader = new StreamReader(HttpContext.Current.Server.MapPath(templateUrl)))
            {
                body = reader.ReadToEnd();
            }
            body = body.Replace("[FullName]", FullName);
            body = body.Replace("[Username]", Username); 
            return body;
        }
        public static string PopulateBodyRegistrationClassNotificationToCustomer(string FullName, string ClassCode, string templateUrl)
        {
            string body = string.Empty;
            using (StreamReader reader = new StreamReader(HttpContext.Current.Server.MapPath(templateUrl)))
            {
                body = reader.ReadToEnd();
            }
            body = body.Replace("[FullName]", FullName);
            body = body.Replace("[ClassCode]", ClassCode);
            return body;
        }
        public static string PopulateBodyRegistrationClassNotificationToAdmin(string FullName, string ClassCode, string Username, string templateUrl)
        {
            string body = string.Empty;
            using (StreamReader reader = new StreamReader(HttpContext.Current.Server.MapPath(templateUrl)))
            {
                body = reader.ReadToEnd();
            }
            body = body.Replace("[FullName]", FullName);
            body = body.Replace("[ClassCode]", ClassCode);
            body = body.Replace("[Username]", Username);
            return body;
        }
        public static string PopulateBodyApprovedOrRejectedClass(string FullName, string ClassCode, string templateUrl)
        {
            string body = string.Empty;
            using (StreamReader reader = new StreamReader(HttpContext.Current.Server.MapPath(templateUrl)))
            {
                body = reader.ReadToEnd();
            }
            body = body.Replace("[FullName]", FullName);
            body = body.Replace("[ClassCode]", ClassCode); 
            return body;
        }
    }
}