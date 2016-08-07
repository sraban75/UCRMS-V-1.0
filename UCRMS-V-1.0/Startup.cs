using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(UCRMS_V_1._0.Startup))]
namespace UCRMS_V_1._0
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
