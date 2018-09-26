﻿using System.IO;
using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using Lucid.Infrastructure.Lib.Testing;
using Microsoft.AspNetCore.Mvc;
using MvcTesting.AspNetCore;
using NUnit.Framework;

namespace Lucid.Infrastructure.Host.Mvc.Tests
{
    [TestFixture]
    public class ViewTests
    {
        [Test]
        public async Task CanSeeModuleViews()
        {
            var client = TestRegistry
                .SetupTestServer<TestStartup>(Path.Combine(TestUtil.ProjectPath(), "../Mvc"))
                .MvcTestingClient();

            var response = await client.GetAsync("/");

            response.HttpStatusCode.Should().Be(HttpStatusCode.OK);
            response.LastResult.Should().BeOfType<ViewResult>();
        }

        public class TestStartup : Startup
        {
            protected override void ConfigureMvcOptions(MvcOptions mvcOptions)
            {
                base.ConfigureMvcOptions(mvcOptions);
                mvcOptions.Filters.Add<CaptureResultFilter>();
            }
        }
    }
}