using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(WeiboDataWithSDK.Startup))]
namespace WeiboDataWithSDK
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
