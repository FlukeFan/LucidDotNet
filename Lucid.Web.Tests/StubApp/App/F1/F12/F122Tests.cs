﻿using FluentAssertions;
using Lucid.Web.Tests.StubApp.Utility;
using NUnit.Framework;

namespace Lucid.Web.Tests.StubApp.App.F1.F12
{
    [TestFixture]
    public class F122Tests : StubAppTest
    {
        [Test]
        public void Index_GetByConvention()
        {
            StubApp.Test(http =>
            {
                var response = http.Get("/f1/f12/f122");

                response.Text.Should().Contain("Response - F1/F12/F122/Index");
            });
        }

        [Test]
        public void Index_GetExplicit()
        {
            StubApp.Test(http =>
            {
                var response = http.Get("/f1/f12/f122/index");

                response.Text.Should().Contain("Response - F1/F12/F122/Index");
            });
        }
    }
}
