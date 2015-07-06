using System;
using FluentAssertions;
using Lucid.Domain.Orgs;
using Lucid.Domain.Orgs.Commands;
using Lucid.Domain.Tests.Utility;
using NUnit.Framework;

namespace Lucid.Domain.Tests.Orgs.Commands
{
    [TestFixture]
    public class LoginTest : DomainTest
    {
        [Test]
        public void Execute()
        {
            var cmd =
                new Login
                {
                    Email = "cmd.test@email.com",
                };

            var userDto = cmd.Execute();

            Repository.ShouldContain<User>(userDto.UserId);
            userDto.Email.Should().Be("cmd.test@email.com");
        }

        [Test]
        public void Validation()
        {
            Func<Login> validCommand = () =>
                new Login
                {
                    Email = "valid.mail@valid.com",
                };

            validCommand().ShouldBeValid();

            validCommand().ShouldBeInvalid(c => c.Email = null);
            validCommand().ShouldBeInvalid(c => c.Email = "");
        }
    }
}
