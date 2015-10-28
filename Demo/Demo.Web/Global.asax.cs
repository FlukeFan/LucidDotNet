using System.Web.Mvc;
using System.Web.Routing;

namespace Demo.Web
{
    public class DemoApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            ViewEngines.Engines.Clear();

            RouteTable.Routes.MapRoute(
                "Default",                                              // Route name
                "{controller}/{action}/{id}",                           // URL with parameters
                new { controller = "Home", action = "Index", id = "" }  // Parameter defaults
            );

            AreaRegistration.RegisterAllAreas();
        }
    }
}