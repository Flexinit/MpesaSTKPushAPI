using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(BuyPamojaMpesaIntegration.Startup))]
namespace BuyPamojaMpesaIntegration
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
