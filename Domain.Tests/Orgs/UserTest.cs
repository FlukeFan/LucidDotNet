using FluentAssertions;
using Lucid.Domain.Orgs;
using Lucid.Domain.Tests.Utility;
using NUnit.Framework;

namespace Lucid.Domain.Tests.Orgs
{
    public class UserBuilder : Builder<User>
    {
    }

    [TestFixture]
    public class UserTest : DomainTest
    {
        [Test]
        public void Login_WhenDoesNotExist_IsCreated()
        {
            var now = SetDomainNow(Test.SummerDateTime1);

            var user = User.Login("does.not@exist.net");

            Repository.ShouldContain(user);

            user.LastLoggedIn.Should().Be(now);
        }
    }
}
