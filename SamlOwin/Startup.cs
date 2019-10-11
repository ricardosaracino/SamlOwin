using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SamlOwin.Startup))]
namespace SamlOwin
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}