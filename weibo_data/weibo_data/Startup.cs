using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(weibo_data.Startup))]
namespace weibo_data
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
