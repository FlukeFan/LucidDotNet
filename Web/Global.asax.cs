using System;
using System.Web;
using System.Web.Routing;
using Lucid.Web.Startup;

namespace Lucid.Web
{
    public class Global : HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }
    }
}