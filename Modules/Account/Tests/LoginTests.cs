using System;
using System.Threading.Tasks;
using FluentAssertions;
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

        [Test]
        public async Task Login_ReturnsNewUser()
        {
            using (new ModuleTestSetup.SetupMemoryLogic())
            {
                var user1 = await new Login { UserName = "Test1" }.ExecuteAsync();
                var user2 = await new Login { UserName = "Test2" }.ExecuteAsync();

                user1.Id.Should().NotBe(0);
                user2.Id.Should().NotBe(0);
                user1.Id.Should().NotBe(user2.Id);
            }
        }

        [Test]
        public async Task Login_ReturnsExistingUser()
        {
            using (new ModuleTestSetup.SetupMemoryLogic())
            {
                var user1 = await new Login { UserName = "Test1" }.ExecuteAsync();
                var user2 = await new Login { UserName = "Test1" }.ExecuteAsync();

                user1.Id.Should().NotBe(0);
                user2.Id.Should().Be(user1.Id, "should return existing user");
            }
        }
    }
}
