using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(AddMFAAuthSample.Startup))]
namespace AddMFAAuthSample
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
