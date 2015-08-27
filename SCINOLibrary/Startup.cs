using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SCINOLibrary.Startup))]
namespace SCINOLibrary
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
