using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(final_project.Startup))]
namespace final_project
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
