using System;
using Lucid.Infrastructure.Lib.Testing.Validation;
using NUnit.Framework;

namespace Lucid.Modules.Account.Tests
{
    [TestFixture]
    public class LoginTests
    {
        private static readonly Func<Login> _validCommand = () => new Login { UserName = "TestUser1" };

        [Test]
        public void Execute()
        {
            var cmd = _validCommand();

            cmd.ShouldBeValid();
        }

        [Test]
        public void Validation()
        {
            _validCommand().ShouldBeInvalid(c => c.UserName = null, "Name cannot be null");
            _validCommand().ShouldBeInvalid(c => c.UserName = "", "Name cannot be empty");
        }
    }
}
