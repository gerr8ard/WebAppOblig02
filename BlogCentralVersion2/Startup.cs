using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(BlogCentralVersion2.Startup))]
namespace BlogCentralVersion2
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
