using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(masterProject.Startup))]
namespace masterProject
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
