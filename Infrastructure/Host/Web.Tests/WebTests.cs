using Lucid.Infrastructure.Host.Web.Layout;
using Lucid.Infrastructure.Lib.MvcApp.Pages;
using Lucid.Infrastructure.Lib.MvcApp.RazorFolders;
using Lucid.Infrastructure.Lib.Testing;
using Lucid.Infrastructure.Lib.Testing.Controller;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using MvcTesting.AspNetCore;
using NUnit.Framework;

namespace Lucid.Infrastructure.Host.Web.Tests
{
    [SetUpFixture]
    public class WebTests
    {
        private static SetupTestServer<TestStartup> _testServerSetup;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            _testServerSetup = new SetupTestServer<TestStartup>();
        }

        [OneTimeTearDown]
        public void OnTimeTearDown()
        {
            using (_testServerSetup) { }
        }

        public abstract class Controller
        {
            [SetUp]
            public void SetUp()
            {
                StubUserFilter.SetupDefault();
            }

            protected SimulatedHttpClient MvcTestingClient() { return _testServerSetup.TestServer.MvcTestingClient(); }
        }

        public class TestStartup : AbstractTestStartup
        {
            protected override ISetLayout NewSetLayout() { return new MenuSetLayout(); }

            public class MenuSetLayout : ISetLayout
            {
                void ISetLayout.Set(IRazorPage page, PageInfo pageInfo, ViewContext viewContext)
                {
                    page.Layout = typeof(MenuModel).RelativeViewPath("Menu.cshtml");
                    viewContext.ViewBag.MenuModel = new MenuModel();
                    viewContext.ViewBag.PageInfo = new PageInfo();
                }
            }
        }
    }
}