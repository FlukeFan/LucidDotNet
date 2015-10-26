using System;
using Demo.Domain.Product.Commands;
using Demo.Domain.Tests.Utility;
using NUnit.Framework;

namespace Demo.Domain.Tests.Product.Commands
{
    [TestFixture]
    public class StartDesignTests
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
            validCommand().ShouldBeInvalid(c => c.Name = new string('x', 256));
        }
    }
}
