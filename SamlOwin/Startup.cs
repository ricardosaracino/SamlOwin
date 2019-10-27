using Microsoft.Owin;
using Owin;
using SamlOwin;

[assembly: OwinStartupAttribute(typeof(Startup))]
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