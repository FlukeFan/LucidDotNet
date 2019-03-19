using System;
using System.Threading.Tasks;
using FluentAssertions;
using Lucid.Infrastructure.Lib.Testing.Execution;
using Lucid.Infrastructure.Lib.Testing.Validation;
using NUnit.Framework;

namespace Lucid.Modules.Security.Tests
{
    [TestFixture]
    public class LoginCommandTests : ModuleTestSetup.LogicTest
    {
        private static readonly Func<LoginCommand> _validCommand = Agreements.Login.Executable;

        [Test]
        public void Validation()
        {
            _validCommand().ShouldBeValid();
            _validCommand().ShouldBeInvalid(c => c.UserName = null, "Name cannot be null");
            _validCommand().ShouldBeInvalid(c => c.UserName = "", "Name cannot be empty");
            _validCommand().ShouldBeInvalid(c => c.UserName = Defaults.LongString, "Name cannot be too long");
        }

        [Test]
        public async Task Agreement()
        {
            var earlier = Registry.UtcNow = () => Defaults.SummerNow;

            var user = await Agreements.Login.Executable().ExecuteAsync();

            user.Should().BeEquivalentTo(Agreements.Login.Result().Mutate(r =>
            {
                r.With(bp => bp.Id, user.Id);
            }));
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
