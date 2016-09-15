﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using FluentAssertions;
using Lucid.Facade.Exceptions;
using NUnit.Framework;

namespace Lucid.Facade.Tests.Exceptions
{
    [TestFixture]
    public class FacadeExceptionTests
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

            var e = new FacadeException(messages, propertyMessages);

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

            var e = new FacadeException(validationResults);

            e.Message.Should().Be("p1: m1\np2: m1\np2: m2\np3: m2");
        }

        [Test]
        public void PropertyErrorExtension()
        {
            var cmd = new PropCommand();

            var exception = cmd.PropertyError(c => c.P1, p => string.Format("Error with {0}", p));

            exception.PropertyMessages.Keys.Should().BeEquivalentTo("P1");
            exception.PropertyMessages["P1"].Should().BeEquivalentTo("Error with P1");
        }

        public class PropCommand
        {
            public string P1 { get; set; }
        }
    }
}
