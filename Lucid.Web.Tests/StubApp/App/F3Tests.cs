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
                var response = http.Get("/f3");

                response.Should().Contain("Response - F3/Index");
            });
        }

        [Test]
        public void Param1_Get()
        {
            StubApp.Test(http =>
            {
                var response = http.Get("/f3/param1/123");

                response.Should().Contain("param1=123");
            });
        }
    }
}
