using System.Configuration;
using Demo.Domain.Utility;
using Demo.Infrastructure.NHibernate;

namespace Demo.Web.Utility
{
    public class Startup
    {
        public virtual void InitRepository()
        {
            // TODO allow overriding using local settings
            var connectionString = ConfigurationManager.AppSettings["connectionString"];

            // verify NH not hitting DB for reserved words
            DemoNhRepository.Init(connectionString, typeof(DemoEntity));
        }
    }
}