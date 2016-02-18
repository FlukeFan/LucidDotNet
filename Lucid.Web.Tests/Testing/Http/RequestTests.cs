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
            request.Headers.Count.Should().Be(0);
            request.FormValues.Count.Should().Be(0);
        }

        [Test]
        public void SetFormValue()
        {
            var request = new Request("/test", "POST").SetFormUrlEncoded();

            request.Headers.Count.Should().Be(1);
            request.Headers.Get("Content-Type").Should().Be("application/x-www-form-urlencoded");

            request.SetFormValue("p1", "123");
            request.SetFormValue("p2", "234");

            request.FormValues.Count.Should().Be(2);
            request.FormValues.Get("p1").Should().Be("123");
            request.FormValues.Get("p2").Should().Be("234");
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
