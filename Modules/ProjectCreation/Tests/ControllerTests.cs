﻿using System.Threading.Tasks;
using FluentAssertions;
using Lucid.Infrastructure.Lib.Testing;
using MvcTesting.Html;
using NUnit.Framework;

namespace Lucid.Modules.ProjectCreation.Tests
{
    public class ControllerTests : ModuleControllerTests<TestStartup>
    {
        [Test]
        public async Task Index_DisplaysForm()
        {
            var form = await MvcTestingClient()
                .GetAsync("/ProjectCreation")
                .Form<IndexModel>();

            form.Method.Should().Be("post");
            form.Action.Should().Be("/projectCreation");

            form.GetText(m => m.Cmd.Name).Should().Be("Demo", "default project generation name should be 'Demo'");
        }
    }
}
