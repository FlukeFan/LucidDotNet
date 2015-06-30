using Lucid.Domain.Orgs;
using Lucid.Domain.Tests.Utility;
using NUnit.Framework;
using FluentAssertions;

namespace Lucid.Domain.Tests.Orgs
{
    [TestFixture]
    public class UserTest : DomainTest
    {
        [Test]
        public void Login_WhenDoesNotExist_IsCreated()
        {
            var now = SetDomainNow(Test.SummerDateTime1);

            var user = User.Login("does.not@exist.net");

            user.Should().NotBeNull();
            user.Id.Should().NotBe(0, "entity not persisted");

            user.LastLoggedIn.Should().Be(now);
        }
    }
}
