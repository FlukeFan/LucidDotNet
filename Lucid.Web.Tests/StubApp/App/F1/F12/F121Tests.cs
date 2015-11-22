﻿using FluentAssertions;
using Lucid.Web.Tests.StubApp.Utility;
using NUnit.Framework;

namespace Lucid.Web.Tests.StubApp.App.F1.F12
{
    [TestFixture]
    public class F121Tests : StubAppTest
    {
        [Test]
        public void Index_GetByConvention()
        {
            StubApp.Test(http =>
            {
                string response = http.Get("/f1/f12/f121");

                response.Should().Contain("F1/F12/F121/Index");
            });
        }

        [Test]
        public void Index_GetExplicit()
        {
            StubApp.Test(http =>
            {
                string response = http.Get("/f1/f12/f121/index");

                response.Should().Contain("F1/F12/F121/Index");
            });
        }
    }
}
