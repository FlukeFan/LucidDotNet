using System.Web.Mvc;
using System.Web.Routing;

namespace Lucid.Web.StubApp.Startup
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute("Default", "", new { controller = "Home", action = "Index" }, new string[] { "Lucid.Web.StubApp.App.Home" });
        }
    }
}