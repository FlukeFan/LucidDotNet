using Demo.Web.Utility;
using Lucid.Web;

namespace Demo.Web
{
    public class DemoApplication : System.Web.HttpApplication
    {
        public static Startup Startup = new Startup();

        protected void Application_Start()
        {
            Startup.InitRepository();

            FeatureFolders.Register(typeof(DemoController).Assembly, "Demo.Web.App");
        }
    }
}