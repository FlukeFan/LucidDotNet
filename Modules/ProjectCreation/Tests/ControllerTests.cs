using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using Lucid.Infrastructure.Lib.Testing;
using Microsoft.AspNetCore.Mvc;
using MvcTesting.Html;
using NUnit.Framework;

namespace Lucid.Modules.ProjectCreation.Tests
{
    public class ExecutorStub : IExecutor
    {
        public IList<object> Executed = new List<object>();

        object IExecutor.Execute(IExecutable executable)
        {
            Executed.Add(executable);
            return null;
        }
    }

    public class ControllerTests : ModuleControllerTests<TestStartup>
    {
        private ExecutorStub _stubExecutor;

        [SetUp]
        public void SetUp()
        {
            _stubExecutor = new ExecutorStub();
            Registry.Executor = _stubExecutor;
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
            var form = await MvcTestingClient()
                .GetAsync("/ProjectCreation")
                .Form<IndexModel>();

            var response = await form
                .SetText(m => m.Cmd.Name, "PostTest")
                .Submit();

            _stubExecutor.Executed.Count.Should().BeGreaterThan(0);
            _stubExecutor.Executed[0].Should().BeEquivalentTo(new GenerateProject { Name = "PostTest" });
        }
    }
}
