using System;
using Demo.Domain.Product;
using Demo.Domain.Product.Commands;
using Demo.Domain.Tests.Utility;
using Demo.Domain.Utility;
using FluentAssertions;
using NUnit.Framework;

namespace Demo.Domain.Tests.Product.Commands
{
    [TestFixture]
    public class StartDesignTests : DomainTest
    {
        [Test]
        public void Validation()
        {
            Func<StartDesign> validCommand = () =>
                new StartDesign
                {
                    Name = "test design",
                };

            validCommand().ShouldBeValid();

            validCommand().ShouldBeInvalid(c => c.Name = null);
            validCommand().ShouldBeInvalid(c => c.Name = "");
            validCommand().ShouldBeInvalid(c => c.Name = new string('x', DemoEntity.DefaultMaxStringLength + 1));
        }

        [Test]
        public void Execute()
        {
            var cmd =
                new StartDesign
                {
                    Name = "created design",
                };

            var designDto = cmd.Execute();

            Repository.ShouldContain<Design>(designDto.DesignId);
            designDto.Name.Should().Be("created design");
        }
    }
}
