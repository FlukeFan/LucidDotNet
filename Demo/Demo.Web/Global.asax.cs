using Demo.Web.Utility;
using Lucid.Web;

namespace Demo.Web
{
    public class DemoApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            FeatureFolders.Register(typeof(DemoController).Assembly, "Demo.Web.App");
        }
    }
}