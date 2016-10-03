using System.IO;
using System.Web.Hosting;
using Lucid.Database;
using Lucid.Infrastructure;
using Lucid.Infrastructure.NHibernate;
using SimpleFacade.Execution;
using SimpleFacade.Validation;

namespace Lucid.Web.Utility
{
    public class Startup
    {
        public virtual void Init()
        {
            InitExecutor();
            InitRepository();
        }

        private void InitExecutor()
        {
            PresentationRegistry.Executor =
                new CqExecutor(
                    new RepositoryExecutor(
                        new ValidatingExecutor(
                            new LucidExecutor()
                        )
                    )
                );
        }

        private void InitRepository()
        {
            var databaseSettings = new DatabaseSettings();
            var settingsFile = Path.Combine(HostingEnvironment.MapPath("~/"), "settings.config");
            Settings.Init(settingsFile, databaseSettings);

            LucidStartup.Init(databaseSettings.LucidConnection);
        }
    }
}