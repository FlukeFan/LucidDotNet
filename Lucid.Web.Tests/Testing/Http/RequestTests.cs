using System.Linq;
using System.Net;
using FluentAssertions;
using Lucid.Web.Testing.Http;
using NUnit.Framework;

namespace Lucid.Web.Tests.Testing.Http
{
    [TestFixture]
    public class RequestTests
    {
        [Test]
        public void Construct()
        {
            var request = new Request("/test/path?p1=123&p2=234");

            request.Url.Should().Be("/test/path");
            request.Query.Should().Be("p1=123&p2=234");
            request.Verb.Should().Be("GET");
            request.ExptectedResponse.Should().Be(HttpStatusCode.OK);
            request.Headers.Count.Should().Be(0);
            request.FormValues.Count().Should().Be(0);
        }

        [Test]
        public void Construct_PostSetFormUrlEncoded()
        {
            var request = new Request("/test", "post");

            request.Verb.Should().Be("POST");
            request.ExptectedResponse.Should().Be(HttpStatusCode.Redirect);

            request.Headers.Count.Should().Be(1, "the content-type header should be set");
            request.Headers.Get("Content-Type").Should().Be("application/x-www-form-urlencoded");
        }

        [Test]
        public void Construct_EmptyExpectedResponse()
        {
            var request = new Request("/test", "PUSH");

            request.ExptectedResponse.Should().BeNull();
        }

        [Test]
        public void AddFormValue()
        {
            var request = new Request("/test", "POST");

            request.AddFormValue("p1", "123");
            request.AddFormValue("p2", "234");

            request.FormValues.ShouldBeEquivalentTo(new NameValue[]
            {
                new NameValue("p1", "123"),
                new NameValue("p2", "234"),
            });
        }

        [Test]
        public void AddQuery()
        {
            var request = new Request("/test/path");

            request.AddQuery("p1", "123");

            request.Query.Should().Be("p1=123");

            request.AddQuery("p2", "234");

            request.Query.Should().Be("p1=123&p2=234");
        }
    }
}
