using System;
using Demo.Domain.Account;
using Demo.Domain.Account.Commands;
using Demo.Domain.Contract.Account.Commands;
using Demo.Domain.Tests.Utility;
using FluentAssertions;
using NUnit.Framework;

namespace Demo.Domain.Tests.Account.Commands
{
    public class LoginTests : DomainTest
    {
        [Test]
        public void UserIsReturned()
        {
            var cmd =
                new Login
                {
                    Email = "cmd.test@email.com",
                };

            var userDto = new LoginHandler().Execute(cmd);

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
