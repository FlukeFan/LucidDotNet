using Lucid.Domain.Orgs;
using NUnit.Framework;

namespace Lucid.Domain.Tests.Orgs
{
    [TestFixture]
    public class UserTest
    {
        [Test]
        public void Login_WhenDoesNotExist_IsCreated()
        {
            var user = User.Login("does.not@exist.net");

            Assert.That(user, Is.Not.Null);
        }
    }
}
