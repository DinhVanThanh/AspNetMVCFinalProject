using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using PrivateTutorOnline.Models;
using System.IO;
using System.Collections.Generic;
using PrivateTutorOnline.Services;
using System.Configuration;

namespace PrivateTutorOnline.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private ApplicationRoleManager _AppRoleManager;
        private TutorOnlineDBContext context;
        private string AdminEmail = ConfigurationManager.AppSettings["AdminEmail"];
        public AccountController()
        {
            context = new TutorOnlineDBContext();
        }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager )
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }
         
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

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = UserManager.Find(model.UserName, model.Password);
            if(user != null)
            {
                Customer customer = context.Customers.SingleOrDefault(c => c.UserId == user.Id);
                if (customer != null)
                {
                    if (customer.IsActivate)
                    {
                        if (customer.IsEnable)
                        {
                            var result = await SignInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, shouldLockout: false);
                            switch (result)
                            {
                                case SignInStatus.Success:
                                    return RedirectToLocal(returnUrl);
                                case SignInStatus.LockedOut:
                                    return View("Lockout");
                                case SignInStatus.RequiresVerification:
                                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                                case SignInStatus.Failure:
                                default:
                                    ModelState.AddModelError("", "Tài khoản hoặc mật khẩu không chính xác.");
                                    return View(model);
                            }
                        }
                        else
                        {
                            ModelState.AddModelError("", "Tài khoản của bạn đã bị khóa.");
                            return View(model);
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "Tài khoản của bạn chưa được kích hoạt");
                        return View(model);
                    }
                }
                else
                {
                    Tutor tutor = context.Tutors.SingleOrDefault(c => c.UserId == user.Id);
                    if (tutor != null)
                    {
                        if (tutor.IsActivate)
                        {
                            if (tutor.IsEnable)
                            {
                                var result = await SignInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, shouldLockout: false);
                                switch (result)
                                {
                                    case SignInStatus.Success:
                                        return RedirectToLocal(returnUrl);
                                    case SignInStatus.LockedOut:
                                        return View("Lockout");
                                    case SignInStatus.RequiresVerification:
                                        return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                                    case SignInStatus.Failure:
                                    default:
                                        ModelState.AddModelError("", "Tài khoản hoặc mật khẩu không chính xác.");
                                        return View(model);
                                }
                            }
                            else
                            {
                                ModelState.AddModelError("", "Tài khoản của bạn đã bị khóa.");
                                return View(model);
                            }
                        }
                        else
                        {
                            ModelState.AddModelError("", "Tài khoản của bạn chưa được kích hoạt");
                            return View(model);
                        }
                    }
                    else
                    {
                        if (user.UserName == "Admin")
                        {
                            var result = await SignInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, shouldLockout: false);
                            switch (result)
                            {
                                case SignInStatus.Success:
                                    return RedirectToLocal(returnUrl);
                                case SignInStatus.LockedOut:
                                    return View("Lockout");
                                case SignInStatus.RequiresVerification:
                                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                                case SignInStatus.Failure:
                                default:
                                    ModelState.AddModelError("", "Tài khoản hoặc mật khẩu không chính xác.");
                                    return View(model);
                            }
                        }
                        else
                        {
                            ModelState.AddModelError("", "Không tồn tại tài khoản này");
                            return View(model);
                        }
                    }
                }
                
            }
            else
            {
                ModelState.AddModelError("", "Tài khoản hoặc mật khẩu không chính xác.");
                return View(model);
            }
            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, change to shouldLockout: true

        }

        
        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RegisterCustomer(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            { 
                var user = new ApplicationUser { UserName = model.Username, Email = model.Email, PhoneNumber = model.PhoneNumber };
                var result =  UserManager.Create(user, model.Password);
                if (result.Succeeded)
                {
                    context.Customers.Add(new Customer() {
                        UserId = user.Id,
                        FullName = model.FullName,
                        PhoneNumber = model.PhoneNumber,
                        Email = model.Email,
                        City = model.City,
                        District = model.District,
                        Street = model.Street,
                        Ward = model.Ward,
                        IsActivate = false,
                        IsEnable = true
                    });
                    await context.SaveChangesAsync();
                    UserManager.AddToRole(user.Id, "Customer");
                    try
                    {
                        //send to customer
                        EmailSenderService.SendHtmlFormattedEmail(model.Email, "Đăng kí tài khoản", EmailSenderService.PopulateBody(model.FullName, model.Username, "~/EmailTemplates/AccountRegisterSuccess.html"));
                        //send to admin
                        EmailSenderService.SendHtmlFormattedEmail(AdminEmail, "Phụ huynh đăng kí tài khoản", EmailSenderService.PopulateBody(model.FullName, model.Username, "~/EmailTemplates/AccountRegisterAdminNotification.html"));
                    }
                    catch(Exception ex)
                    {
                        ModelState.AddModelError("", "Tài khoản của bạn đã được tạo thành công ! Vui lòng kiểm tra Email ");
                        return RedirectToAction("Login", "Account");
                    } 
                    // For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=320771
                    // Send an email with this link
                    // string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    // var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    // await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");
                    ModelState.AddModelError("", "Tài khoản của bạn đã được tạo thành công ! Vui lòng kiểm tra Email ");
                    return RedirectToAction("Login", "Account");
                }
                else
                {
                    return RedirectToAction("Register", "Account");
                }
                 
            }

            // If we got this far, something failed, redisplay form
            return RedirectToAction("Register", "Account");
        }
       
        [HttpPost]
        [AllowAnonymous] 
        public async Task<ActionResult> RegisterTutor(HttpPostedFileBase Avatar, PrivateTutorOnline.Models.BindingModels.TutorBindingModel tutorInfo)
        {
            
            var user = new ApplicationUser { UserName = tutorInfo.Username, Email = tutorInfo.Email, PhoneNumber = tutorInfo.PhoneNumber };
            var result = UserManager.Create(user, tutorInfo.Password);
            if (result.Succeeded)
            {
                UserManager.AddToRole(user.Id, "Tutor");
                //await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                Tutor tutor = new Tutor();
                tutor.UserId = user.Id;
                tutor.FullName = tutorInfo.FullName;
                tutor.City = tutorInfo.City;
                tutor.District = tutorInfo.District;
                tutor.Ward = tutorInfo.Ward;
                tutor.Street = tutorInfo.Street;
                tutor.Advantage = tutorInfo.Advantage;
                if(tutorInfo.DateOfBirth.HasValue)
                {
                    tutor.DateOfBirth = tutorInfo.DateOfBirth.Value;
                }
                tutor.Gender = tutorInfo.Gender;
                tutor.Degree = tutorInfo.Degree;
                tutor.Email = tutorInfo.Email;
                tutor.GraduationYear = tutorInfo.GraduationYear;
                tutor.HomeTown = tutorInfo.HomeTown;
                tutor.IdentityNumber = tutorInfo.IdentityNumber;
                tutor.MajorSubject = tutorInfo.MajorSubject;
                tutor.PhoneNumber = tutorInfo.PhoneNumber;
                tutor.University = tutorInfo.UniversityName;
                if(Avatar != null && Avatar.ContentLength > 0)
                {
                    tutor.Image = new byte[Avatar.ContentLength];
                    Avatar.InputStream.Read(tutor.Image, 0, Avatar.ContentLength);
                } 
                tutor.Subjects = new List<Subject>();
                tutor.Grades = new List<Grade>();
                foreach (int i in tutorInfo.Subjects)
                {
                    tutor.Subjects.Add(context.Subjects.SingleOrDefault(s => s.Id == i));
                }
                foreach (int i in tutorInfo.Grades)
                {
                    tutor.Grades.Add(context.Grades.SingleOrDefault(gr => gr.Id == i));
                }
                tutor.IsActivate = false;
                tutor.IsEnable = true;
                context.Tutors.Add(tutor);
                context.SaveChanges();
                try
                {
                    //send to tutor
                    EmailSenderService.SendHtmlFormattedEmail(tutorInfo.Email, "Đăng kí tài khoản", EmailSenderService.PopulateBody(tutorInfo.FullName, tutorInfo.Username, "~/EmailTemplates/AccountRegisterSuccess.html"));
                    //send to admin
                    EmailSenderService.SendHtmlFormattedEmail(AdminEmail, "Gia sư đăng kí tài khoản", EmailSenderService.PopulateBody(tutorInfo.FullName, tutorInfo.Username, "~/EmailTemplates/AccountRegisterAdminNotification.html"));
                }
                catch(Exception ex)
                {
                    ModelState.AddModelError("", "Tài khoản của bạn đã được tạo thành công ! Vui lòng kiểm tra Email ");
                    return RedirectToAction("Login", "Account");
                }

                ModelState.AddModelError("", "Tài khoản của bạn đã được tạo thành công ! Vui lòng kiểm tra Email ");
                return RedirectToAction("Login", "Account");
            }
            else
                return RedirectToAction("TutorRegistrationForm", "Tutors");
           
        }

         
        //
        // GET: /Customer/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }
               
        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Login", "Account");
        }
         

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
        }

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
    }
}