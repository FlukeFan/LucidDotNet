using System;
using System.Web.Mvc;
using FluentAssertions;
using Lucid.Web.Testing.Http;
using NUnit.Framework;

namespace Lucid.Web.Tests.Testing.Http
{
    [TestFixture]
    public class ResponseTests
    {
        [Test]
        public void ActionResult_WhenLastResultNull_Throws()
        {
            var response = new Response { LastResult = null };

            var e = Assert.Throws<Exception>(() => response.ActionResult());

            e.Message.Should().Be("Expected ActionResult, but got no result from global filter CaptureResultFilter");
        }

        [Test]
        public void ActionResultOf_WhenActionResultNull_Throws()
        {
            var response = new Response { LastResult = new ResultExecutedContext { Result = null } };

            var e = Assert.Throws<Exception>(() => response.ActionResultOf<ActionResult>());

            e.Message.Should().Be("Expected ActionResult, but got <null>");
        }

        [Test]
        public void ActionResultOf_WhenIncorrectType_Throws()
        {
            var response = new Response { LastResult = new ResultExecutedContext { Result = new FileContentResult(new byte[0], "content-type") } };

            var e = Assert.Throws<Exception>(() => response.ActionResultOf<FilePathResult>());

            e.Message.Should().Be("Expected FilePathResult, but got FileContentResult");
        }
    }
}
