using System;
using Demo.Domain.Account;
using Demo.Domain.Account.Commands;
using Demo.Domain.Tests.Utility;
using FluentAssertions;
using NUnit.Framework;

namespace Demo.Domain.Tests.Account.Commands
{
    [TestFixture]
    public class LoginTests : DomainTest
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
