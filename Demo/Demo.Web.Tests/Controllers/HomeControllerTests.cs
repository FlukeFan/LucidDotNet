using Demo.Web.Tests.Utility;
using NUnit.Framework;

namespace Demo.Web.Tests.Controllers
{
    public class HomeControllerTests : WebTest
    {
        [Test]
        public void Index_RendersView()
        {
            WebApp.Test(client =>
            {
            });
        }
    }
}
