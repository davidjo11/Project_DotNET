using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Project_DotNET.Startup))]
namespace Project_DotNET
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
