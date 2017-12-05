using PrivateTutorOnline.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PrivateTutorOnline.Services;
using PrivateTutorOnline.Models.ViewModels;

namespace PrivateTutorOnline.Controllers
{
    [Authorize(Roles ="Admin")]
    public class AdminController : Controller
    {
        TutorOnlineDBContext db = new TutorOnlineDBContext();
        private string AdminEmail = "tieuluantotnghiep2017@gmail.com";
        // GET: Admin
        public ActionResult CustomerManagementView(string searchString, bool? IsSeachById, int? page)
        {
            IList<Customer> Customers = null;
            if(!String.IsNullOrEmpty(searchString))
            {
                if(IsSeachById.Value)
                {
                    int searchStringId = -1;
                    if (int.TryParse(searchString, out searchStringId))
                        searchStringId = Int32.Parse(searchString);
                    Customers = db.Customers.Where(
                    s => s.Id == searchStringId 
                    ).ToList();
                }
                else
                {
                    int searchStringId = -1;
                    if (int.TryParse(searchString, out searchStringId))
                        searchStringId = Int32.Parse(searchString);
                    Customers = db.Customers.Where(
                    s =>  s.FullName.Contains(searchString)
                    || s.PhoneNumber.Equals(searchString)
                    || s.Email.Contains(searchString)
                    || s.City.Contains(searchString)
                    || s.District.Contains(searchString)
                    || s.Ward.Contains(searchString)
                    || s.Street.Contains(searchString)
                    ).ToList();
                }
                
            }
            else
            {
                Customers = db.Customers.ToList();
            }
            return View(new CustomerManagementViewModel() { Customers = Customers.ToPagedList<Customer>(page.HasValue ? page.Value : 1, 2), searchString = searchString });
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
        public ActionResult TutorManagementView(string searchString, bool? IsSeachById, int? page)
        {
            IList<Tutor> Tutors = null;
            if (!String.IsNullOrEmpty(searchString))
            {
                if(IsSeachById.Value)
                {
                    int searchStringId = -1;
                    if (int.TryParse(searchString, out searchStringId))
                        searchStringId = Int32.Parse(searchString);
                    Tutors = db.Tutors.Include(s => s.Grades).Include(s => s.Subjects).Where(
                    s => s.Id == searchStringId 
                    ).ToList();
                }
                else
                {
                    int searchStringId = -1;
                    if (int.TryParse(searchString, out searchStringId))
                        searchStringId = Int32.Parse(searchString);
                    Tutors = db.Tutors.Include(s => s.Grades).Include(s => s.Subjects).Where(
                    s =>  s.FullName.Contains(searchString)
                    || s.PhoneNumber.Equals(searchString)
                    || s.Email.Contains(searchString)
                    || s.City.Contains(searchString)
                    || s.District.Contains(searchString)
                    || s.Ward.Contains(searchString)
                    || s.Street.Contains(searchString)
                    ).ToList();
                }
               
            }
            else
            {
                Tutors = db.Tutors.Include(s => s.Grades).Include(s => s.Subjects).ToList();
            }
            return View( new TutorManagementViewModel() { Tutors = Tutors.ToPagedList<Tutor>(page.HasValue ? page.Value : 1, 2), searchString = searchString });
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
        public ActionResult ClassManagementView(string searchString, bool? IsSeachById, int? page)
        {
            IList<RegistrationClass> Classes = null;
            
            if (!String.IsNullOrEmpty(searchString))
            {
                if(IsSeachById.Value)
                {
                    int searchStringId = -1;
                    if (int.TryParse(searchString, out searchStringId))
                        searchStringId = Int32.Parse(searchString);
                    Classes = db.RegistrationClasses.Include(s => s.Tutor).Include(s => s.Customer).Include(s => s.Subjects).Where(
                    s => s.Id == searchStringId 
                    ).ToList();
                }
                else
                {
                    int searchStringId = -1;
                    if (int.TryParse(searchString, out searchStringId))
                        searchStringId = Int32.Parse(searchString);
                    Classes = db.RegistrationClasses.Include(s => s.Tutor).Include(s => s.Customer).Include(s => s.Subjects).Where(
                    s =>  
                     s.Customer.FullName.Contains(searchString)
                    || s.Tutor.FullName.Equals(searchString)
                    || s.TutoringTime.Contains(searchString)
                    || s.Grade.Name.Contains(searchString)
                    || s.Subjects.Any(t => t.Name.Contains(searchString))
                    || s.City.Contains(searchString)
                    || s.District.Contains(searchString)
                    || s.Ward.Contains(searchString)
                    || s.Street.Contains(searchString)
                    ).ToList();
                }
            }
            else
            {
                Classes = db.RegistrationClasses.Include(s => s.Tutor).Include(s => s.Customer).Include(s => s.Subjects).ToList();
            }
            return View(new ClassManagementViewModel() { RegistrationClasses = Classes.OrderByDescending(s => s.Id).ToPagedList<RegistrationClass>(page.HasValue ? page.Value : 1, 2), searchString = searchString });

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
                RegistrationClass Class = db.RegistrationClasses.Include(s => s.Customer).SingleOrDefault(s => s.Id == ClassId);
                Customer customer = Class.Customer;
                if (IsApproved)
                {
                    Class.Status = Enums.ClassStatus.AdminApproved;
                    if(Class.Customer != null)
                    {
                        EmailSenderService.SendHtmlFormattedEmail(Class.Customer.Email, "Lớp đã được duyệt",
                            EmailSenderService.PopulateBodyApprovedOrRejectedClass(Class.Customer.FullName, ClassId.ToString() , "~/EmailTemplates/ClassRegistrationApprovedNotification.html"));
                    }
                    
                }
                    
                else
                {
                    Class.Status = Enums.ClassStatus.AdminReject;
                    if (Class.Customer != null)
                    {
                        EmailSenderService.SendHtmlFormattedEmail(Class.Customer.Email, "Lớp bị từ chối",
                        EmailSenderService.PopulateBodyApprovedOrRejectedClass(Class.Customer.FullName, ClassId.ToString() , "~/EmailTemplates/ClassRegistrationRejectedNotification.html"));


                    }
                }
                    
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
        public ActionResult SubjectManagement(string searchString, bool? IsSeachById, int? page)
        {
            IList<Subject> subjects = null;
            if (!String.IsNullOrEmpty(searchString))
            {
                if(IsSeachById.Value)
                {
                    int searchStringId = -1;
                    if (int.TryParse(searchString, out searchStringId))
                        searchStringId = Int32.Parse(searchString);
                    subjects = db.Subjects.Include(s => s.Tutors).Where(s => s.Id == searchStringId).ToList();
                }
                else
                {
                    int searchStringId = -1;
                    if (int.TryParse(searchString, out searchStringId))
                        searchStringId = Int32.Parse(searchString);
                    subjects = db.Subjects.Include(s => s.Tutors).Where(
                    s => s.Name.Contains(searchString) || s.Tutors.Any(t => t.FullName.Contains(searchString))).ToList();
                }
            }
            else
            {
                subjects = db.Subjects.Include(s => s.Tutors).ToList();
            }
            return View(new SubjectManagementViewModel() { Subjects = subjects.ToPagedList(page.HasValue ? page.Value : 1, 5), searchString = searchString });
        }
        // GET: Admin/Details/5
        public ActionResult GradeManagement(string searchString, bool? IsSeachById, int? page)
        {
            IList<Grade> grades = null;
            if (!String.IsNullOrEmpty(searchString))
            {
                if(IsSeachById.Value)
                {
                    int searchStringId = -1;
                    if (int.TryParse(searchString, out searchStringId))
                        searchStringId = Int32.Parse(searchString);
                    grades = db.Grades.Include(s => s.Tutors).Where(s => s.Id == searchStringId).ToList();
                }
                else
                {
                    int searchStringId = -1;
                    if (int.TryParse(searchString, out searchStringId))
                        searchStringId = Int32.Parse(searchString);
                    grades = db.Grades.Include(s => s.Tutors).Where(
                    s =>  s.Name.Contains(searchString)
                    || s.Tutors.Any(t => t.FullName.Contains(searchString))
                    ).ToList();
                }
                
            }
            else
            {
                grades = db.Grades.Include(s => s.Tutors).ToList();
            }
            return View(new GradeManagementViewModel() { Grades = grades.ToPagedList(page.HasValue ? page.Value : 1, 5), searchString = searchString });
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
