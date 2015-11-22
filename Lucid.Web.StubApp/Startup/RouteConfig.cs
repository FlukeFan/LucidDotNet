using System.Web.Mvc;
using System.Web.Routing;

namespace Lucid.Web.StubApp.Startup
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute("F121",     "f1/f12/f121/{action}",     new { controller = "F121",  action = "Index" }, new string[] { "Lucid.Web.StubApp.App.F1.F12.F121" });
            routes.MapRoute("F122",     "f1/f12/f122/{action}",     new { controller = "F122",  action = "Index" }, new string[] { "Lucid.Web.StubApp.App.F1.F12.F122" });
            routes.MapRoute("F11",      "f1/f11/{action}",          new { controller = "F11",   action = "Index" }, new string[] { "Lucid.Web.StubApp.App.F1.F11" });
            routes.MapRoute("F1X",      "f1/Home/{action}",         new { controller = "Home",  action = "Index" }, new string[] { "Lucid.Web.StubApp.App.F1.Home" });
            routes.MapRoute("F1",       "f1/{action}",              new { controller = "Home",  action = "Index" }, new string[] { "Lucid.Web.StubApp.App.F1.Home" });
            routes.MapRoute("F2",       "f2/{action}",              new { controller = "F2",    action = "Index" }, new string[] { "Lucid.Web.StubApp.App.F2" });
            routes.MapRoute("F3",       "f3/{action}",              new { controller = "F3",    action = "Index" }, new string[] { "Lucid.Web.StubApp.App.F3" });
            routes.MapRoute("RX",       "Home/{action}",            new { controller = "Home",  action = "Index" }, new string[] { "Lucid.Web.StubApp.App.Home" });
            routes.MapRoute("R",        "{action}",                 new { controller = "Home",  action = "Index" }, new string[] { "Lucid.Web.StubApp.App.Home" });
        }
    }
}