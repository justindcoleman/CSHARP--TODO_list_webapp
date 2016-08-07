using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Week8WeekendHomework.Startup))]
namespace Week8WeekendHomework
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
