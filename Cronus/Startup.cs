using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Cronus.Startup))]
namespace Cronus
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            //ConfigureAuth(app);
        }
    }
}
