using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ShoppingListTeam3.Startup))]
namespace ShoppingListTeam3
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
