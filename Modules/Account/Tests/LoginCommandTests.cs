using System;
using System.Threading.Tasks;
using FluentAssertions;
using Lucid.Infrastructure.Lib.Testing.Validation;
using NUnit.Framework;

namespace Lucid.Modules.Account.Tests
{
    [TestFixture]
    public class LoginCommandTests : ModuleTestSetup.LogicTest
    {
        private static readonly Func<LoginCommand> _validCommand = () => new LoginCommand { UserName = "TestUser1" };

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
            var user1 = await new LoginCommand { UserName = "Test1" }.ExecuteAsync();
            var user2 = await new LoginCommand { UserName = "Test2" }.ExecuteAsync();

            user1.Id.Should().NotBe(0);
            user2.Id.Should().NotBe(0);
            user1.Id.Should().NotBe(user2.Id);
        }

        [Test]
        public async Task Login_ReturnsExistingUser()
        {
            var user1 = await new LoginCommand { UserName = "Test1" }.ExecuteAsync();
            var user2 = await new LoginCommand { UserName = "Test1" }.ExecuteAsync();

            user1.Id.Should().NotBe(0);
            user2.Id.Should().Be(user1.Id, "should return existing user");
        }

        [Test]
        public async Task Login_UpdatesLastLoggedIn()
        {
            var earlier = Registry.UtcNow = () => new DateTime(2008, 07, 06);

            var user = await new LoginCommand { UserName = "Test1" }.ExecuteAsync();

            user.LastLoggedIn.Should().Be(earlier());

            var later = Registry.UtcNow = () => new DateTime(2009, 08, 07);

            user = await new LoginCommand { UserName = user.Name }.ExecuteAsync();

            user.LastLoggedIn.Should().Be(later());
        }
    }
}
