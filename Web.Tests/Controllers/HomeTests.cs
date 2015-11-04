using System;
using System.IO;
using FluentAssertions;
using Lucid.Web.Testing.Hosting;
using NUnit.Framework;

namespace Web.Tests.Controllers
{
    [TestFixture]
    public class HomeTests
    {
        private AspNetTestHost _host;

        [SetUp]
        public void SetUp()
        {
            _host = AspNetTestHost.For(Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, @"..\..\..\Web")));
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
