using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using Microsoft.Owin;
using PrivateTutorOnline.Models;
using PrivateTutorOnline.Models.BindingModels;
using System;
using System.Collections.Generic;
using System.Linq; 
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Microsoft.Owin.Host;
using System.Net;

namespace PrivateTutorOnline.Controllers
{
    public class AccountController : Controller
    {
        private TutorOnlineDBContext context;
        public AccountController()
        {
            context = new TutorOnlineDBContext();
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public  Task<ActionResult> Register(RegisterBindingModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser() { UserName = model.Username };
                UserManager<ApplicationUser> userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
                var result = userManager.Create(user, model.Password);
                if (result.Succeeded)
                {
                    
                    return Task.FromResult<ActionResult> (RedirectToAction("Index", "Home"));
                } 
            }

            // If we got this far, something failed, redisplay form
            return Task.FromResult<ActionResult>(View(model));
        }
         
    } 
}