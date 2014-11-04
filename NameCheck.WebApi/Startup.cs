using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(NameCheck.WebApi.Startup))]

namespace NameCheck.WebApi
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}