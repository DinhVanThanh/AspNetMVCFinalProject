using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PrivateTutorOnline.Models;
using PrivateTutorOnline.Models.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using PrivateTutorOnline.Services;
using PagedList;

namespace PrivateTutorOnline.Controllers
{
    [Authorize(Roles = "Admin, Customer, Tutor")]
    public class ManageRegistrationClassesController : Controller
    {
        private TutorOnlineDBContext db = new TutorOnlineDBContext();
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private string AdminEmail = "tieuluantotnghiep2017@gmail.com";
        public ApplicationRoleManager AppRoleManager
        {
            get
            {
                return HttpContext.GetOwinContext().Get<ApplicationRoleManager>();
            }
        }
        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        // GET: ManageRegistrationClasses
        public async Task<ActionResult> Index()
        {
            return View(await db.RegistrationClasses.ToListAsync());
        }
        public async Task<ActionResult> PostedClass(string searchString, bool? IsSeachById, int? page)
        {
            string UserId = User.Identity.GetUserId();
            IList<RegistrationClass> postedClass = db.RegistrationClasses.Include(t => t.Grade).Include(t => t.Subjects).Where(s => s.Customer.UserId == UserId && !s.IsClosed).ToList();
            if (!String.IsNullOrEmpty(searchString))
            {
                if(IsSeachById.Value)
                {
                    int searchStringId = -1;
                    if (int.TryParse(searchString, out searchStringId))
                        searchStringId = Int32.Parse(searchString);
                    postedClass = postedClass.Where(
                    s => s.Id == searchStringId
                    ).ToList();
                }
                else
                {
                    int searchStringId = -1;
                    if (int.TryParse(searchString, out searchStringId))
                        searchStringId = Int32.Parse(searchString);
                    postedClass = postedClass.Where(
                    s =>  s.Requirement.Contains(searchString)
                    || s.Subjects.Any(t => t.Name.Contains(searchString))
                    || s.Grade.Name.Contains(searchString)
                    || s.City.Contains(searchString)
                    || s.District.Contains(searchString)
                    || s.Ward.Contains(searchString)
                    || s.Street.Contains(searchString)
                    || s.Requirement.Contains(searchString)
                    || s.TutoringTime.Contains(searchString)
                    ).ToList();
                }
                
            } 
            IList<PostedClassViewModel> postedClassViewModel = new List<PostedClassViewModel>();
            foreach (var item in postedClass)
            {
                postedClassViewModel.Add(new PostedClassViewModel()
                {
                    Id = item.Id,
                    Grade = item.Grade,
                    Subjects = item.Subjects,
                    SalaryPerMonth = item.SalaryPerMonth,
                    DayPerWeek = item.DayPerWeek,
                    TutoringTime = item.TutoringTime,
                    Requirement = item.Requirement,
                    Status = item.Status,
                    City = item.City,
                    District = item.District,
                    Ward = item.Ward,
                    Street = item.Street,
                    Tutor = item.Tutor,
                    ReceivedDate = item.ReceivedDate
                });
            }
            IPagedList<PostedClassViewModel> Postclass = postedClassViewModel.OrderByDescending(s => s.Id).ToPagedList(page ?? 1, 2);
            return View(new PagedListPostedClassViewModel() { PostedClasses = Postclass, searchString = searchString });
        } 
        public async Task<ActionResult> EnrolledClass(string searchString, bool? IsSeachById, int? page)
        {
            string UserId = User.Identity.GetUserId();
            IList<RegistrationClass> enrolledClass;
            if (User.IsInRole("Tutor"))
            { 
                 enrolledClass = db.RegistrationClasses.Include(t => t.Grade).Include(t => t.Subjects).Include(t => t.Customer).Where(s => s.Tutor.UserId == UserId && s.IsClosed).ToList();
            }
            else if(User.IsInRole("Customer"))
            {
                 enrolledClass = db.RegistrationClasses.Include(t => t.Grade).Include(t => t.Subjects).Include(t => t.Tutor).Where(s => s.Customer.UserId == UserId && s.IsClosed).ToList();
            }
            else
            {
                 enrolledClass = new List<RegistrationClass>();
            }
            if (!String.IsNullOrEmpty(searchString))
            {
                if( IsSeachById.Value)
                {
                    int searchStringId = -1;
                    if (int.TryParse(searchString, out searchStringId))
                        searchStringId = Int32.Parse(searchString);
                    enrolledClass = enrolledClass.Where(
                    s => s.Id == searchStringId
                    ).ToList();
                }
                else
                {
                    int searchStringId = -1;
                    if (int.TryParse(searchString, out searchStringId))
                        searchStringId = Int32.Parse(searchString);
                    enrolledClass = enrolledClass.Where(
                    s =>  s.Requirement.Contains(searchString)
                    || s.Subjects.Any(t => t.Name.Contains(searchString))
                    || s.Grade.Name.Contains(searchString)
                    || s.City.Contains(searchString)
                    || s.District.Contains(searchString)
                    || s.Ward.Contains(searchString)
                    || s.Street.Contains(searchString)
                    || s.Requirement.Contains(searchString)
                    || s.TutoringTime.Contains(searchString)
                    || User.IsInRole("Customer") ? s.Tutor.FullName.Contains(searchString) : s.Customer.FullName.Contains(searchString)
                    ).ToList();
                }
                
            } 
            IList<PostedClassViewModel> enrolledClassViewModel = new List<PostedClassViewModel>();
            foreach (var item in enrolledClass)
            {
                enrolledClassViewModel.Add(new PostedClassViewModel()
                {
                    Id = item.Id,
                    Grade = item.Grade,
                    Subjects = item.Subjects,
                    SalaryPerMonth = item.SalaryPerMonth,
                    DayPerWeek = item.DayPerWeek,
                    TutoringTime = item.TutoringTime,
                    Requirement = item.Requirement,
                    Status = item.Status,
                    City = item.City,
                    District = item.District,
                    Ward = item.Ward,
                    Street = item.Street,
                    ReceivedDate = item.ReceivedDate,
                    Tutor = item.Tutor,
                    Customer = item.Customer
                });
            }
            IPagedList<PostedClassViewModel> Postclass = enrolledClassViewModel.OrderByDescending(s => s.Id).ToPagedList(page ?? 1, 2);
            return View(new PagedListPostedClassViewModel() { PostedClasses = Postclass, searchString = searchString }); 
        }
       
        public async Task<ActionResult> AllPostedClass(string searchString, bool? IsSeachById, int? page)
        {
            
            IList<RegistrationClass> registrationClasses = db.RegistrationClasses.Include(t => t.Grade).Include(t => t.Subjects).Where(t => !t.IsClosed && t.Status != Enums.ClassStatus.AdminReject && t.Status != Enums.ClassStatus.WaitingForAdminApproval).ToList();
            if (!String.IsNullOrEmpty(searchString))
            {
                if(IsSeachById.Value)
                {
                    int searchStringId = -1;
                    if (int.TryParse(searchString, out searchStringId))
                        searchStringId = Int32.Parse(searchString);
                    registrationClasses = registrationClasses.Where(
                    s => s.Id == searchStringId
                    ).ToList();
                }
                else
                {
                    int searchStringId = -1;
                    if (int.TryParse(searchString, out searchStringId))
                        searchStringId = Int32.Parse(searchString);
                    registrationClasses = registrationClasses.Where(
                    s => s.Requirement.Contains(searchString)
                    || s.Subjects.Any(t => t.Name.Contains(searchString))
                    || s.Grade.Name.Contains(searchString)
                    || s.City.Contains(searchString)
                    || s.District.Contains(searchString)
                    || s.Ward.Contains(searchString)
                    || s.Street.Contains(searchString)
                    || s.Requirement.Contains(searchString)
                    || s.TutoringTime.Contains(searchString)
                    ).ToList();
                }
                
            } 
            List<PostedClassViewModel> allPostedClass = new List<PostedClassViewModel>();
            foreach (var item in registrationClasses)
            {
                allPostedClass.Add(new PostedClassViewModel()
                {
                    Id = item.Id,
                    Grade = item.Grade,
                    Subjects = item.Subjects,
                    SalaryPerMonth = item.SalaryPerMonth,
                    DayPerWeek = item.DayPerWeek,
                    TutoringTime = item.TutoringTime,
                    Requirement = item.Requirement,
                    Status = item.Status,
                    City = item.City,
                    District = item.District,
                    Ward = item.Ward,
                    Street = item.Street,
                    ReceivedDate = item.ReceivedDate
                });
            }
            IPagedList<PostedClassViewModel> Postclass = allPostedClass.OrderByDescending(s => s.Id).ToPagedList(page ?? 1, 2);
            return View(new PagedListPostedClassViewModel() { PostedClasses = Postclass, searchString = searchString });
        }
        [HttpPost]
        public HttpStatusCodeResult ReceiveClass(int ClassId)
        {
            try
            {
                string tutorUsername = User.Identity.GetUserName(); 
                string UserId = User.Identity.GetUserId(); 
                RegistrationClass RegistrationClass = db.RegistrationClasses.Include(s => s.Customer).SingleOrDefault(s => s.Id == ClassId);
                Customer customer = RegistrationClass.Customer;
                string customerUsername = UserManager.FindById(customer.UserId).UserName;
                Tutor tutor = db.Tutors.SingleOrDefault(s => s.UserId == UserId);
                RegistrationClass.Tutor = tutor;
                RegistrationClass.Status = Enums.ClassStatus.WaitingForCustomerApproval;
                
                db.Entry(RegistrationClass).State = EntityState.Modified;
                db.SaveChanges();
                //send to tutor
                EmailSenderService.SendHtmlFormattedEmail(tutor.Email, "Yêu cầu nhận lớp mã số : " + RegistrationClass.Id,
                            EmailSenderService.PopulateBody(customer.FullName, tutor.FullName, RegistrationClass.Id.ToString() , "~/EmailTemplates/ClassTutorEnrollmentNotificationToTutor.html"));
                //send to customer
                EmailSenderService.SendHtmlFormattedEmail(customer.Email, "Yêu cầu nhận lớp",
                            EmailSenderService.PopulateBodyTutorEnrollClassNotificationToCustomer(customer.FullName, tutor, RegistrationClass.Id.ToString(), "~/EmailTemplates/ClassTutorEnrollmentNotificationToCustomer.html"));
                //send to admin
                EmailSenderService.SendHtmlFormattedEmail(AdminEmail, "Gia sư đăng kí nhận lớp",
                            EmailSenderService.PopulateBodyTutorEnrollClassNotificationToAdmin(customer, customerUsername, tutor, tutorUsername, RegistrationClass.Id.ToString(), "~/EmailTemplates/ClassTutorEnrollmentNotificationToAdmin.html"));
            }
            catch(Exception ex)
            {
                return new HttpStatusCodeResult( HttpStatusCode.InternalServerError);
            }
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }
        [HttpPost]
        public HttpStatusCodeResult ApproveOrRejectEnrollClass(int ClassId, bool IsApproved)
        {
            try
            {
                string customerUsername = User.Identity.GetUserName();  
                RegistrationClass RegistrationClass = db.RegistrationClasses.Include(s => s.Customer).Include(s => s.Tutor).Include(s => s.Subjects).SingleOrDefault(s => s.Id == ClassId);
                Tutor tutor = RegistrationClass.Tutor;
                string tutorUsername = UserManager.FindById(tutor.UserId).UserName;
                Customer customer = RegistrationClass.Customer;
                if (IsApproved)
                {
                    RegistrationClass.Tutor = tutor;
                    RegistrationClass.Status = Enums.ClassStatus.CustomerApproved;
                    RegistrationClass.ReceivedDate = DateTime.Now;
                    RegistrationClass.IsClosed = true;
                    db.Entry(RegistrationClass).State = EntityState.Modified;
                    db.SaveChanges();
                    //send to tutor
                    EmailSenderService.SendHtmlFormattedEmail(tutor.Email, "Đã duyệt yêu cầu nhận lớp" + RegistrationClass.Id,
                                EmailSenderService.PopulateBodyTutorCustomerApprovedEnrollmentRequestToTutor(customer, tutor, RegistrationClass, "~/EmailTemplates/CustomerApprovedTutorEnrollmenToTutor.html"));
                   
                    //send to admin
                    EmailSenderService.SendHtmlFormattedEmail(AdminEmail, "Yêu cầu gia sư nhận lớp được duyệt",
                                EmailSenderService.PopulateBodyTutorCustomerApprovedEnrollmentRequestToAdmin(customer, customerUsername, tutor, tutorUsername, RegistrationClass.Id.ToString(), "~/EmailTemplates/CustomerApprovedTutorEnrollmentToAdmin.html"));
                }
                else
                { 
                    RegistrationClass.Status = Enums.ClassStatus.CustomerRejected; 
                    db.Entry(RegistrationClass).State = EntityState.Modified;
                    db.SaveChanges();
                    //send to tutor
                    EmailSenderService.SendHtmlFormattedEmail(tutor.Email, "Từ chối yêu cầu nhận lớp mã số : " + RegistrationClass.Id,
                                EmailSenderService.PopulateBody(customer.FullName, tutor.FullName, RegistrationClass.Id.ToString(), "~/EmailTemplates/CustomerRejectedTutorEnrollmentToTutor.html"));
                   
                }
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        // GET: ManageRegistrationClasses/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RegistrationClass registrationClass = await db.RegistrationClasses.FindAsync(id);
            if (registrationClass == null)
            {
                return HttpNotFound();
            }
            return View(registrationClass);
        }

        // GET: ManageRegistrationClasses/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ManageRegistrationClasses/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,SalaryPerMonth,DayPerWeek,TutoringTime,Requirement,ReceivedDate,Status,City,District,Ward,Street,IsActive")] RegistrationClass registrationClass)
        {
            if (ModelState.IsValid)
            {
                db.RegistrationClasses.Add(registrationClass);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(registrationClass);
        }

        // GET: ManageRegistrationClasses/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RegistrationClass registrationClass = await db.RegistrationClasses.FindAsync(id);
            if (registrationClass == null)
            {
                return HttpNotFound();
            }
            return View(registrationClass);
        }

        // POST: ManageRegistrationClasses/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,SalaryPerMonth,DayPerWeek,TutoringTime,Requirement,ReceivedDate,Status,City,District,Ward,Street,IsActive")] RegistrationClass registrationClass)
        {
            if (ModelState.IsValid)
            {
                db.Entry(registrationClass).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(registrationClass);
        }

        // GET: ManageRegistrationClasses/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RegistrationClass registrationClass = await db.RegistrationClasses.FindAsync(id);
            if (registrationClass == null)
            {
                return HttpNotFound();
            }
            return View(registrationClass);
        }

        // POST: ManageRegistrationClasses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            RegistrationClass registrationClass = await db.RegistrationClasses.FindAsync(id);
            db.RegistrationClasses.Remove(registrationClass);
            await db.SaveChangesAsync();
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
