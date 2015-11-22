using FluentAssertions;
using Lucid.Web.Testing.Hosting;
using NUnit.Framework;

namespace Lucid.Web.Tests.StubApp.App
{
    [TestFixture]
    public class HomeTests
    {
        private AspNetTestHost _host;

        [SetUp]
        public void SetUp()
        {
            _host = AspNetTestHost.For(@"..\..\..\Lucid.Web.StubApp");
        }

        [TearDown]
        public void TearDown()
        {
            using (_host) { }
        }

        [Test]
        public void Home_Get()
        {
            _host.Test(http =>
            {
                string response = http.Get("/");

                response.Should().Contain("first controller");
            });
        }
    }
}
