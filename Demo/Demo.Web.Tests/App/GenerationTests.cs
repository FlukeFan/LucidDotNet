using Demo.Web.App.Generation;
using Demo.Web.Tests.Utility;
using FluentAssertions;
using NUnit.Framework;

namespace Demo.Web.Tests.App
{
    public class GenerationTests : WebTest
    {
        [Test]
        public void Generate_GET_RendersForm()
        {
            WebAppTest(client =>
            {
                var response = client.Get(Actions.Generate());

                response.Text.Should().Contain("<form");
            });
        }
    }
}
