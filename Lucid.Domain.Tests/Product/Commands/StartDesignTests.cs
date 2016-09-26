using System;
using Lucid.Domain.Contract;
using Lucid.Domain.Contract.Product.Commands;
using Lucid.Domain.Product;
using Lucid.Domain.Product.Commands;
using Lucid.Domain.Tests.Utility;
using FluentAssertions;
using NUnit.Framework;

namespace Lucid.Domain.Tests.Product.Commands
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
            validCommand().ShouldBeInvalid(c => c.Name = new string('x', Constraints.DefaultMaxStringLength + 1));
        }

        [Test]
        public void NewDesignIsReturned()
        {
            var cmd =
                new StartDesign
                {
                    Name = "created design",
                };

            var designDto = new StartDesignHandler().Execute(cmd);

            Repository.ShouldContain<Design>(designDto.DesignId);
            designDto.Name.Should().Be("created design");
        }
    }
}
