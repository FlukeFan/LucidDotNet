using System.IO;
using System.Web.Hosting;
using System.Web.Mvc;
using Lucid.Database;
using Lucid.Infrastructure;
using Lucid.Infrastructure.NHibernate;
using MvcForms;
using SimpleFacade.Execution;
using SimpleFacade.Validation;

namespace Lucid.Web.Utility
{
    public class Startup
    {
        public virtual void Init()
        {
            GlobalFilters.Filters.Add(new CookieAuthentication(
                App.Home.Actions.Login(),
                SkipAuthentication,
                LucidUser.CreateFromCookieValue));

            GlobalFilters.Filters.Add(new PjaxFilter());

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

        private bool SkipAuthentication(ControllerContext context)
        {
            var controllerType = context.Controller.GetType();
            var isHome = controllerType == typeof(App.Home.HomeController);
            var isHtml = controllerType == typeof(App.Html.HtmlController);

            return isHome || isHtml;
        }
    }
}