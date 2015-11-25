using System;
using System.Web;
using System.Web.Routing;
using Lucid.Web.StubApp.Startup;

namespace Lucid.Web.StubApp
{
    public class Global : HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }
    }
}