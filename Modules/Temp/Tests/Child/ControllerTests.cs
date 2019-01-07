using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using Lucid.Infrastructure.Lib.Testing;
using NUnit.Framework;

namespace Lucid.Modules.Temp.Tests.Child
{
    [TestFixture]
    public class ControllerTests : ModuleControllerTests<TestStartup>
    {
        [Test]
        public async Task Index()
        {
            var response = await MvcTestingClient()
                .GetAsync(Temp.Child.Actions.Index());

            response.HttpStatusCode.Should().Be(HttpStatusCode.OK);
            response.Text.Should().Contain("precompiled child");
        }
    }
}
