using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using FluentAssertions;
using Lucid.Domain.Exceptions;
using Lucid.Domain.Validation;
using NUnit.Framework;

namespace Lucid.Domain.Tests.Validation
{
    [TestFixture]
    public class LucidValidatorTests
    {
        [Test]
        public void Validate_ThrowsWhenInvalidField()
        {
            Action act = () =>
            {
                var dto = new Dto { Name = null, Description = null };
                LucidValidator.Validate(dto);
            };

            var e = act.ShouldThrow<LucidException>().Which;

            e.Messages.Count().Should().Be(0);
            e.PropertyMessages.Count.Should().Be(2);

            e.PropertyMessages.Keys.Should().BeEquivalentTo("Description", "Name");

            e.PropertyMessages["Name"].Should().BeEquivalentTo("The Name field is required.");
            e.PropertyMessages["Description"].Should().BeEquivalentTo("The Description field is required.");
        }

        public class Dto
        {
            [Required]
            public string Name { get; set; }

            [Required]
            public string Description { get; set; }
        }
    }
}
