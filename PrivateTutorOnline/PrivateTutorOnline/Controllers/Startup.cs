using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.AspNet.Identity;

[assembly: OwinStartup(typeof(PrivateTutorOnline.Controllers.Startup))]

namespace PrivateTutorOnline.Controllers
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=316888
            CookieAuthenticationOptions options = new CookieAuthenticationOptions();
            options.AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie;
            options.LoginPath = new PathString("/Login/Login");
            app.UseCookieAuthentication(options);
        }
    }
}
