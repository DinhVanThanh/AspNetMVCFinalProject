using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(PrivateTutorOnline.Startup))]
namespace PrivateTutorOnline
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
