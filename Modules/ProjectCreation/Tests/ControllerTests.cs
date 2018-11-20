﻿using System.Net;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Lucid.Infrastructure.Lib.Testing;
using Lucid.Infrastructure.Lib.Testing.Execution;
using Microsoft.AspNetCore.Mvc;
using MvcTesting.Html;
using NUnit.Framework;

namespace Lucid.Modules.ProjectCreation.Tests
{
    public class ControllerTests : ModuleControllerTests<TestStartup>
    {
        private ExecutorStub _stubExecutor;

        [SetUp]
        public void SetUp()
        {
            Registry.Executor = _stubExecutor = new ExecutorStub();
        }

        [Test]
        public async Task Root_RedirectsToProjectCreation()
        {
            var response = await MvcTestingClient().GetAsync("/");

            response.StatusCode.Should().Be(HttpStatusCode.Redirect);
            response.ActionResultOf<RedirectResult>().Url.Should().Be("projectCreation");
        }

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

        [Test]
        public async Task Index_Post_GeneratesProject()
        {
            var expectedBytes = Encoding.ASCII.GetBytes("stub bytes");
            _stubExecutor.StubResult<GenerateProject>(expectedBytes);

            var form = await MvcTestingClient()
                .GetAsync("/ProjectCreation")
                .Form<IndexModel>();

            var response = await form
                .SetText(m => m.Cmd.Name, "PostTest")
                .Submit();

            _stubExecutor.Executed.Count.Should().BeGreaterThan(0);
            _stubExecutor.Executed[0].Should().BeEquivalentTo(new GenerateProject { Name = "PostTest" });

            var fileResult = response.ActionResultOf<FileResult>();
            fileResult.ContentType.Should().Be("application/zip");
            fileResult.FileDownloadName.Should().Be("PostTest.zip");
        }
    }
}
