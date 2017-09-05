using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(miniSmart.Startup))]
namespace miniSmart
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
