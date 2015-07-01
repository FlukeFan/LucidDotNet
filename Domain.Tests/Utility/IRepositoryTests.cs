using FluentAssertions;
using Lucid.Domain.Orgs;
using Lucid.Domain.Tests.Orgs;
using Lucid.Domain.Utility;
using NUnit.Framework;

namespace Lucid.Domain.Tests.Utility
{
    [TestFixture]
    public abstract class IRepositoryTests
    {
        abstract protected IRepository New();

        [Test]
        public void Save_SetsId()
        {
            var repository = New();
            var user = new UserBuilder().Value();

            user.Id.Should().Be(0, "newly instantiated entity should have a default Id value");

            var savedUser = repository.Save(user);

            savedUser.Should().BeSameAs(user);
            savedUser.Id.Should().NotBe(0, "persisted entity should have a non-zero Id");
        }

        [Test]
        public void Query_RetrieveAll()
        {
            var repository = New();

            var user1 = new UserBuilder().Save();
            var user2 = new UserBuilder().Save();

            var allUsers = repository.Query<User>().List();

            allUsers.Count.Should().Be(2);
            allUsers.Should().Contain(user1);
            allUsers.Should().Contain(user2);
        }
    }
}
