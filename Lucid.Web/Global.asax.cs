using Lucid.Web.Utility;
using MvcFolders;

namespace Lucid.Web
{
    public class LucidApplication : System.Web.HttpApplication
    {
        public static Startup Startup = new Startup();

        protected void Application_Start()
        {
            Startup.Init();

            FeatureFolders.Register(typeof(LucidController).Assembly, "Lucid.Web.App");
        }
    }
}