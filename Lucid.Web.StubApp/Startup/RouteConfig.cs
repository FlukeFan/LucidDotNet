using System.Web.Mvc;
using System.Web.Routing;
using Lucid.Web.StubApp.App.Home;

namespace Lucid.Web.StubApp.Startup
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            var featureFolders = new LucidFeatureFolders(typeof(HomeController).Assembly, "Lucid.Web.StubApp.App");
            routes.Add("LucidFeatureFolders", featureFolders.Route);
        }
    }
}