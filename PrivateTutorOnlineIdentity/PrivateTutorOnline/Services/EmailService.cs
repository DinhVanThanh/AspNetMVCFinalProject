using PrivateTutorOnline.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Web;
using PrivateTutorOnline.Extensions;
using System.Net;

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

        public static string PopulateBodyChooseTutor(Tutor tutor, Customer customer, RegistrationClass registrationClass, string templateUrl)
        {
            string body = string.Empty;
            using (StreamReader reader = new StreamReader(HttpContext.Current.Server.MapPath(templateUrl)))
            {
                body = reader.ReadToEnd();
            }
            body = body.Replace("[TutorFullName]", tutor.FullName);
            body = body.Replace("[CustomerFullName]", customer.FullName);
            body = body.Replace("[ClassCode]", registrationClass.Id.ToString());
            body = body.Replace("[GradeName]", tutor.Id.ToString());
            string SubjectNames = "";
            foreach (var item in registrationClass.Subjects)
            {

                SubjectNames += " - " + item.Name;
            }
            body = body.Replace("[SubjectNames]", SubjectNames);
            body = body.Replace("[SalaryPerMonth]", registrationClass.SalaryPerMonth);
            body = body.Replace("[TutoringTime]", registrationClass.TutoringTime);
            body = body.Replace("[DayPerWeek]", registrationClass.DayPerWeek.ToString());
            body = body.Replace("[Street]", registrationClass.Street);
            body = body.Replace("[Ward]", registrationClass.Ward);
            body = body.Replace("[District]", registrationClass.District);
            body = body.Replace("[City]", registrationClass.City);
            return body;
            return body;
        }
        public static string PopulateBodyTutorEnrollClassNotificationToAdmin(Customer customer, string customerUsername, Tutor tutor, string tutorUsername, string ClassCode, string templateUrl)
        {
            string body = string.Empty;
            using (StreamReader reader = new StreamReader(HttpContext.Current.Server.MapPath(templateUrl)))
            {
                body = reader.ReadToEnd();
            }
            body = body.Replace("[TutorCode]", tutor.Id.ToString());
            body = body.Replace("[TutorFullName]", tutor.FullName);
            body = body.Replace("[TutorUserName]", tutorUsername);
            body = body.Replace("[CustomerCode]", customer.Id.ToString());
            body = body.Replace("[CustomerFullName]", customer.FullName);
            body = body.Replace("[CustomerUserName]", customerUsername);
            body = body.Replace("[ClassCode]", ClassCode);
            return body;
        }
        public static string PopulateBodyTutorCustomerApprovedEnrollmentRequestToAdmin(Customer customer, string customerUsername, Tutor tutor, string tutorUsername, string ClassCode, string templateUrl)
        {
            string body = string.Empty;
            using (StreamReader reader = new StreamReader(HttpContext.Current.Server.MapPath(templateUrl)))
            {
                body = reader.ReadToEnd();
            }
            body = body.Replace("[TutorCode]", tutor.Id.ToString());
            body = body.Replace("[TutorFullName]", tutor.FullName);
            body = body.Replace("[TutorUserName]", tutorUsername);
            body = body.Replace("[CustomerCode]", customer.Id.ToString());
            body = body.Replace("[CustomerFullName]", customer.FullName);
            body = body.Replace("[CustomerUserName]", customerUsername);
            body = body.Replace("[ClassCode]", ClassCode);
            return body;
        }
        public static string PopulateBodyTutorCustomerApprovedEnrollmentRequestToTutor(Customer customer,  Tutor tutor, RegistrationClass registrationClass, string templateUrl)
        {
            string body = string.Empty;
            using (StreamReader reader = new StreamReader(HttpContext.Current.Server.MapPath(templateUrl)))
            {
                body = reader.ReadToEnd();
            } 
            body = body.Replace("[TutorFullName]", tutor.FullName); 
            body = body.Replace("[CustomerFullName]", customer.FullName); 
            body = body.Replace("[ClassCode]", registrationClass.Id.ToString());
            body = body.Replace("[GradeName]", tutor.Id.ToString());
            string SubjectNames = "";
            foreach(var item in registrationClass.Subjects)
            {
                
                SubjectNames += " - " + item.Name ;
            }
            body = body.Replace("[SubjectNames]", SubjectNames);
            body = body.Replace("[SalaryPerMonth]", registrationClass.SalaryPerMonth); 
            body = body.Replace("[TutoringTime]", registrationClass.TutoringTime);
            body = body.Replace("[DayPerWeek]", registrationClass.DayPerWeek.ToString());
            body = body.Replace("[Street]", registrationClass.Street);
            body = body.Replace("[Ward]", registrationClass.Ward);
            body = body.Replace("[District]", registrationClass.District);
            body = body.Replace("[City]", registrationClass.City);
            return body;
        }
        public static string PopulateBodyTutorEnrollClassNotificationToCustomer(string FullName, Tutor tutor, string ClassCode, string templateUrl)
        {
            string body = string.Empty;
            using (StreamReader reader = new StreamReader(HttpContext.Current.Server.MapPath(templateUrl)))
            {
                body = reader.ReadToEnd();
            }
            body = body.Replace("[TutorCode]", tutor.Id.ToString());
            body = body.Replace("[TutorFullName]", tutor.FullName);
            body = body.Replace("[FullName]", FullName);
            body = body.Replace("[ClassCode]", ClassCode);
            body = body.Replace("[Degree]", tutor.Degree.GetDegreeName());
            body = body.Replace("[Gender]", tutor.Gender.GetGenderName());
            body = body.Replace("[University]", tutor.University);
            body = body.Replace("[MajorSubject]", tutor.MajorSubject);
            body = body.Replace("[GraduationYear]", tutor.GraduationYear);
            body = body.Replace("[Image]", tutor.Image.ToString()); 
            return body;
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