using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Zeje.Staticize_.Startup))]
namespace Zeje.Staticize_
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
