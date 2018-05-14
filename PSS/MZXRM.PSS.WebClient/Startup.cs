using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MZXRM.PSS.WebClient.Startup))]
namespace MZXRM.PSS.WebClient
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
