using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PrivateTutorOnline.Auths
{
    public class CustomAuthorizeAttribute : AuthorizeAttribute
    {
        PrivateTutorOnline.Models.TutorOnlineDBContext context = new Models.TutorOnlineDBContext(); // my entity  
        private readonly string[] allowedroles = new string[] { "Admin"};
        public CustomAuthorizeAttribute(params string[] roles)
        {
            this.allowedroles = roles;
        }
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            bool authorize = false;
            foreach (var role in allowedroles)
            {
                 
            }
            return authorize;
        }
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new HttpUnauthorizedResult();
        }
    }
}