using PrivateTutorOnline.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PrivateTutorOnline.Services;

namespace PrivateTutorOnline.Controllers
{
    [Authorize(Roles ="Admin")]
    public class AdminController : Controller
    {
        TutorOnlineDBContext db = new TutorOnlineDBContext();
        private string AdminEmail = "tieuluantotnghiep2017@gmail.com";
        // GET: Admin
        public ActionResult CustomerManagementView(int? page)
        {
            IList<Customer> Customers = db.Customers.ToList();
            return View(Customers.ToPagedList<Customer>(page.HasValue ? page.Value : 1, 2));
        }
        [HttpPost]
        public JsonResult ActivateCustomer(int CustomerId)
        {
            try
            {
                Customer customer = db.Customers.SingleOrDefault(s => s.Id == CustomerId);
                if (!customer.IsActivate)
                    customer.IsActivate = true;
                db.Entry(customer).State = EntityState.Modified;
                db.SaveChanges();
                EmailSenderService.SendHtmlFormattedEmail(customer.Email, "Kích hoạt tài khoản", EmailSenderService.PopulateBody(customer.FullName, "~/EmailTemplates/ActivateAccountSuccess.html"));
            }
            catch (Exception ex)
            {
                return Json(new { Status = "Error" });
            }
            
            return Json(new { Status = "OK" });
        }
        [HttpPost]
        public JsonResult DisableCustomer(int CustomerId)
        {
            try
            {
                Customer customer = db.Customers.SingleOrDefault(s => s.Id == CustomerId);
                if (!customer.IsEnable)
                {
                    customer.IsEnable = true;
                    EmailSenderService.SendHtmlFormattedEmail(customer.Email, "Mở tài khoản", EmailSenderService.PopulateBody(customer.FullName, "~/EmailTemplates/EnableAccountNotification.html"));
                } 
                else
                {
                    EmailSenderService.SendHtmlFormattedEmail(customer.Email, "Khóa tài khoản", EmailSenderService.PopulateBody(customer.FullName, "~/EmailTemplates/DisableAccountNotification.html"));
                    customer.IsEnable = false;
                }
                    
                db.Entry(customer).State = EntityState.Modified;
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                return Json(new { Status = "Error" });
            }
            
            return Json(new { Status = "OK" });
        }
        public ActionResult TutorManagementView(int? page)
        {
            IList<Tutor> Tutors = db.Tutors.Include(s => s.Grades).Include(s => s.Subjects).ToList();
            return View(Tutors.ToPagedList<Tutor>(page.HasValue ? page.Value : 1, 2));
        }
        [HttpPost]
        public JsonResult ActivateTutor(int TutorId)
        {
            try
            {
                Tutor tutor = db.Tutors.SingleOrDefault(s => s.Id == TutorId);
                if (!tutor.IsActivate)
                    tutor.IsActivate = true;
                db.Entry(tutor).State = EntityState.Modified;
                db.SaveChanges();
                EmailSenderService.SendHtmlFormattedEmail(tutor.Email, "Kích hoạt tài khoản", EmailSenderService.PopulateBody(tutor.FullName, "~/EmailTemplates/ActivateAccountSuccess.html"));
            }
            catch(Exception ex)
            {
                return Json(new { Status = "Error" });
            }
            return Json(new { Status = "OK" });
        }
        [HttpPost]
        public JsonResult DisableTutor(int TutorId)
        {
            try
            {
                Tutor tutor = db.Tutors.SingleOrDefault(s => s.Id == TutorId);
                if (!tutor.IsEnable)
                {
                    tutor.IsEnable = true;
                    EmailSenderService.SendHtmlFormattedEmail(tutor.Email, "Mở tài khoản", EmailSenderService.PopulateBody(tutor.FullName, "~/EmailTemplates/EnableAccountNotification.html"));
                } 
                else
                {
                    tutor.IsEnable = false;
                    EmailSenderService.SendHtmlFormattedEmail(tutor.Email, "Khóa tài khoản", EmailSenderService.PopulateBody(tutor.FullName, "~/EmailTemplates/DisableAccountNotification.html"));
                } 
                db.Entry(tutor).State = EntityState.Modified;
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                return Json(new { Status = "Error" });
            } 
            
            return Json(new { Status = "OK" });
        }
        public ActionResult ClassManagementView(int? page)
        {
            IList<RegistrationClass> Classes = db.RegistrationClasses.Include(s => s.Tutor).Include(s => s.Customer).ToList();
            return View(Classes.ToPagedList<RegistrationClass>(page.HasValue ? page.Value : 1, 2));
        }
        public ActionResult DeleteClassRegistration(int id)
        {
            RegistrationClass Class = db.RegistrationClasses.SingleOrDefault(s => s.Id == id);
            if(Class != null)
            {
                Class.Grade = null;
                Class.Subjects = null;
                Class.Tutor = null;
                Class.Customer = null;
                
                db.RegistrationClasses.Remove(Class);
                db.SaveChanges();
            } 
            
            return RedirectToAction("ClassManagementView", "Admin");
        }
        [HttpPost]
        public JsonResult ApproveOrRejectClass(int ClassId, bool IsApproved)
        {
            try
            {
                RegistrationClass Class = db.RegistrationClasses.SingleOrDefault(s => s.Id == ClassId);
                if (IsApproved)
                    Class.Status = Enums.ClassStatus.AdminApproved;
                else
                    Class.Status = Enums.ClassStatus.Reject;
                db.Entry(Class).State = EntityState.Modified;
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                return Json(new { Status = "Error" });
            }

            return Json(new { Status = "OK" });
        }
        // GET: Admin/Details/5
        public ActionResult SubjectManagement(int? page)
        {
            return View(db.Subjects.ToList().ToPagedList(page.HasValue ? page.Value : 1, 5));
        }
        // GET: Admin/Details/5
        public ActionResult GradeManagement(int? page)
        {
            return View(db.Grades.ToList().ToPagedList(page.HasValue ? page.Value : 1, 5));
        }
        public ActionResult SubjectManagementEditView(int id)
        {
            return View("SubjectManagementEdit", db.Subjects.SingleOrDefault(s => s.Id == id));
        }
        public ActionResult GradeManagementEditView(int id)
        {
            return View("GradeManagementEdit", db.Grades.SingleOrDefault(s => s.Id == id));
        }
        public ActionResult SubjectManagementEdit(Subject subjectEdit)
        {
            try
            {
                Subject subject = db.Subjects.SingleOrDefault(s => s.Id == subjectEdit.Id);
                subject.Name = subjectEdit.Name;
                db.Entry(subject).State = EntityState.Modified;
                db.SaveChanges();

            }
            catch (Exception ex)
            {
                return RedirectToAction("Error");
            } 
            return RedirectToAction("SubjectManagement", "Admin");
        }
        public ActionResult GradeManagementEdit(Grade gradeEdit)
        {
            try
            {
                Grade grade = db.Grades.SingleOrDefault(s => s.Id == gradeEdit.Id);
                grade.Name = gradeEdit.Name;
                db.Entry(grade).State = EntityState.Modified;
                db.SaveChanges();
                
            }
           catch(Exception ex)
            {
                return RedirectToAction("Error");
            }
            return RedirectToAction("GradeManagement", "Admin");
        }
        public ActionResult SubjectManagementDelete(int id)
        {
            db.Subjects.Remove(db.Subjects.SingleOrDefault(s => s.Id == id));
            db.SaveChanges();
            return RedirectToAction("SubjectManagement", "Admin");
        }
        public ActionResult GradeManagementDelete(int id)
        {
            db.Grades.Remove(db.Grades.SingleOrDefault(s => s.Id == id));
            db.SaveChanges();
            return RedirectToAction("GradeManagement", "Admin");
        }
        public ActionResult SubjectManagementCreateView()
        {
            return View();
        }
        public ActionResult GradeManagementCreateView()
        {
            return View();
        }
        [HttpPost]
        public ActionResult SubjectManagementCreate([Bind(Include = "Name")] Subject newSubject)
        {
            db.Subjects.Add(new Subject() { Name = newSubject.Name });
            db.SaveChanges();
            return RedirectToAction("SubjectManagement", "Admin");
        }
        [HttpPost]
        public ActionResult GradeManagementCreate([Bind(Include = "Name")] Grade newGrade)
        {
            db.Grades.Add(new Grade() { Name = newGrade.Name });
            db.SaveChanges();
            return RedirectToAction("GradeManagement", "Admin");
        }
        // GET: Admin/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Admin/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Admin/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Admin/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Admin/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Admin/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
