using System.Configuration;
using Demo.Domain.Utility;
using Demo.Infrastructure.NHibernate;
using Demo.Web.Utility;
using Lucid.Web;

namespace Demo.Web
{
    public class DemoApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            if (!WebHost.IsRunningInTestHost)
            {
                // TODO allow overriding using local settings
                var connectionString = ConfigurationManager.AppSettings["connectionString"];

                // verify NH not hitting DB for reserved words
                DemoNhRepository.Init(connectionString, typeof(DemoEntity));
            }

            FeatureFolders.Register(typeof(DemoController).Assembly, "Demo.Web.App");
        }
    }
}