using System.Web.Mvc;
using System.Web.Routing;

namespace Lucid.Web.StubApp.Startup
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute("Default",  "",     new { controller = "Home",  action = "Index" }, new string[] { "Lucid.Web.StubApp.App.Home" });
            routes.MapRoute("F1",       "F1",   new { controller = "F1",    action = "Index" }, new string[] { "Lucid.Web.StubApp.App.F1" });
            routes.MapRoute("F2",       "F2",   new { controller = "F2",    action = "Index" }, new string[] { "Lucid.Web.StubApp.App.F2" });
            routes.MapRoute("F3",       "F3",   new { controller = "F3",    action = "Index" }, new string[] { "Lucid.Web.StubApp.App.F3" });
        }
    }
}