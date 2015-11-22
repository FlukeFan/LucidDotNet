using FluentAssertions;
using Lucid.Web.Tests.StubApp.Utility;
using NUnit.Framework;

namespace Lucid.Web.Tests.StubApp.App
{
    [TestFixture]
    public class F3Tests : StubAppTest
    {
        [Test]
        public void Index_Get()
        {
            StubApp.Test(http =>
            {
                string response = http.Get("/f3");

                response.Should().Contain("F3/Index");
            });
        }
    }
}
