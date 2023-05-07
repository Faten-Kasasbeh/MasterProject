using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Master_Project.Startup))]
namespace Master_Project
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
