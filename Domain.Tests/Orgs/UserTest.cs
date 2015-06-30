using Lucid.Domain.Orgs;
using Lucid.Domain.Tests.Utility;
using NUnit.Framework;

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

            Assert.That(user, Is.Not.Null);
            Assert.That(user.Id, Is.Not.EqualTo(0), "failed: entity not persisted");

            Assert.That(user.LastLoggedIn, Is.EqualTo(now));
        }
    }
}
