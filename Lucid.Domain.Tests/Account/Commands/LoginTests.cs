using System;
using FluentAssertions;
using Lucid.Domain.Account;
using Lucid.Domain.Account.Commands;
using Lucid.Domain.Contract.Account.Commands;
using Lucid.Domain.Tests.Utility;
using NUnit.Framework;

namespace Lucid.Domain.Tests.Account.Commands
{
    public class LoginTests : DomainTest
    {
        [Test]
        public void UserIsReturned()
        {
            var cmd =
                new Login
                {
                    Name = "CmdTest",
                };

            var userDto = new LoginHandler().Execute(cmd);

            Repository.ShouldContain<User>(userDto.UserId);
            userDto.Name.Should().Be("CmdTest");
        }

        [Test]
        public void Validation()
        {
            Func<Login> validCommand = () =>
                new Login
                {
                    Name = "ValidName",
                };

            validCommand().ShouldBeValid();

            validCommand().ShouldBeInvalid(c => c.Name = null);
            validCommand().ShouldBeInvalid(c => c.Name = "");
        }
    }
}
