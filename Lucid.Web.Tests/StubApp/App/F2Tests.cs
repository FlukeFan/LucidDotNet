﻿using FluentAssertions;
using Lucid.Web.Tests.StubApp.Utility;
using NUnit.Framework;

namespace Lucid.Web.Tests.StubApp.App
{
    [TestFixture]
    public class F2Tests : StubAppTest
    {
        [Test]
        public void Index_Get()
        {
            StubApp.Test(http =>
            {
                var response = http.Get("/f2");

                response.Text.Should().Contain("Response - F2/Index");
            });
        }
    }
}
