using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using FluentAssertions;
using Lucid.Domain.Exceptions;
using NUnit.Framework;

namespace Lucid.Domain.Tests.Exceptions
{
    [TestFixture]
    public class LucidExceptionTests
    {
        [Test]
        public void FormatMessage()
        {
            var messages = new List<string>
            {
                "m1",
                "m2",
            };

            var propertyMessages = new Dictionary<string, IList<string>>
            {
                { "p1", new List<string> { "m1", "m2" } },
                { "p2", new List<string> { "m1" } },
            };

            var e = new LucidException(messages, propertyMessages);

            e.Message.Should().Be("m1\nm2\np1: m1\np1: m2\np2: m1");
        }

        [Test]
        public void ValidationResults()
        {
            var validationResults = new List<ValidationResult>
            {
                new ValidationResult("m1", new List<string> { "p1", "p2" }),
                new ValidationResult("m2", new List<string> { "p2", "p3" }),
            };

            var e = new LucidException(validationResults);

            e.Message.Should().Be("p1: m1\np2: m1\np2: m2\np3: m2");
        }
    }
}
