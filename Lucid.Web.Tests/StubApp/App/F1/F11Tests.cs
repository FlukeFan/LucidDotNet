﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Lucid.Web.Tests.StubApp.Utility;
using NUnit.Framework;

namespace Lucid.Web.Tests.StubApp.App.F1
{
    [TestFixture]
    public class F11Tests : StubAppTest
    {
        [Test]
        public void Index_GetByConvention()
        {
            StubApp.Test(http =>
            {
                string response = http.Get("/f1/f11");

                response.Should().Contain("F1/F11/Index");
            });
        }

        [Test]
        public void Index_GetExplicit()
        {
            StubApp.Test(http =>
            {
                string response = http.Get("/f1/f11/index");

                response.Should().Contain("F1/F11/Index");
            });
        }
    }
}
