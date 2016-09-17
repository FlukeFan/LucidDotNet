using Demo.Web.Utility;
using MvcFolders;

namespace Demo.Web
{
    public class DemoApplication : System.Web.HttpApplication
    {
        public static Startup Startup = new Startup();

        protected void Application_Start()
        {
            Startup.Init();

            FeatureFolders.Register(typeof(DemoController).Assembly, "Demo.Web.App");
        }
    }
}