using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(DevEnv_Semester_Project.Startup))]
namespace DevEnv_Semester_Project
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
