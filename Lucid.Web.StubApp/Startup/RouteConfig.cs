using System.Web.Mvc;
using System.Web.Routing;

namespace Lucid.Web.StubApp.Startup
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute("F1X",      "f1/Home/{action}",         new { controller = "Home",  action = "Index" }, new string[] { "Lucid.Web.StubApp.App.F1.Home" });
            routes.MapRoute("F1",       "f1/{action}",              new { controller = "Home",  action = "Index" }, new string[] { "Lucid.Web.StubApp.App.F1.Home" });
            routes.MapRoute("F2",       "f2/{action}",              new { controller = "F2",    action = "Index" }, new string[] { "Lucid.Web.StubApp.App.F2" });
            routes.MapRoute("F3",       "f3/{action}",              new { controller = "F3",    action = "Index" }, new string[] { "Lucid.Web.StubApp.App.F3" });
            routes.MapRoute("RX",       "Home/{action}",            new { controller = "Home",  action = "Index" }, new string[] { "Lucid.Web.StubApp.App.Home" });
            routes.MapRoute("R",        "{action}",                 new { controller = "Home",  action = "Index" }, new string[] { "Lucid.Web.StubApp.App.Home" });
        }
    }
}