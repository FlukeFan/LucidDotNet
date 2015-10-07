using Demo.Domain.Orgs;
using FluentAssertions;
using Lucid.Domain.Persistence;
using Lucid.Domain.Persistence.Queries;
using Lucid.Domain.Tests.Orgs;
using NUnit.Framework;

namespace Lucid.Domain.Tests.Utility
{
    [TestFixture]
    public abstract class IRepositoryTests
    {
        abstract protected IRepository<int> New();

        private IRepository<int> _repository;

        [SetUp]
        public virtual void SetUp()
        {
            _repository = New();
        }

        [TearDown]
        public virtual void TearDown()
        {
            _repository = null;
        }

        [Test]
        public void Save_SetsId()
        {
            var user = new UserBuilder().Value();

            user.Id.Should().Be(0, "newly instantiated entity should have a default Id value");

            var savedUser = _repository.Save(user);

            savedUser.Should().BeSameAs(user);
            savedUser.Id.Should().NotBe(0, "persisted entity should have a non-zero Id");
        }

        [Test]
        public void Query_RetrieveAll()
        {
            var user1 = new UserBuilder().Save();
            var user2 = new UserBuilder().Save();

            var allUsers = _repository.Query<User>().List();

            allUsers.Count.Should().Be(2);
            allUsers.Should().Contain(user1);
            allUsers.Should().Contain(user2);
        }

        [Test]
        public void Query_RestrictPropertyEqual()
        {
            var user1 = new UserBuilder().With(u => u.Email, "test1@user.net").Save();
            var user2 = new UserBuilder().With(u => u.Email, "test2@user.net").Save();

            var specificUser =
                _repository.Query<User>()
                    .Filter(Where<User>.PropEq(u => u.Email, "test2@user.net"))
                    .SingleOrDefault();

            specificUser.Should().NotBeNull();
            specificUser.Should().BeSameAs(user2);
        }
    }
}
