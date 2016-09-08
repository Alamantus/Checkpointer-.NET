using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Checkpointer.Startup))]
namespace Checkpointer
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            
        }
    }
}
