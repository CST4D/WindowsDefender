using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(DatabaseSiteTest.Startup))]
namespace DatabaseSiteTest
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
