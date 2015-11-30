using System;
using System.Web;
using Lucid.Web.StubApp.App.Home;

namespace Lucid.Web.StubApp
{
    public class Global : HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            FeatureFolders.Register(typeof(HomeController).Assembly, "Lucid.Web.StubApp.App");
        }
    }
}