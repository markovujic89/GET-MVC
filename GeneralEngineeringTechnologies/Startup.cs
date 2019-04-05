using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(GeneralEngineeringTechnologies.Startup))]
namespace GeneralEngineeringTechnologies
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
