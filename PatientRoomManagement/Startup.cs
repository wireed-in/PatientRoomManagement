using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(PatientRoomManagement.Startup))]
namespace PatientRoomManagement
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
