using FluentAssertions;
using Lucid.Domain.Persistence;
using Lucid.Domain.Persistence.Queries;
using NUnit.Framework;

namespace Lucid.Domain.Tests.Persistence
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
            var user = new LucidEntityBuilder().Value();

            user.Id.Should().Be(0, "newly instantiated entity should have a default Id value");

            var savedUser = _repository.Save(user);

            savedUser.Should().BeSameAs(user);
            savedUser.Id.Should().NotBe(0, "persisted entity should have a non-zero Id");
        }

        [Test]
        public void Query_RetrieveAll()
        {
            var user1 = new LucidEntityBuilder().Save(_repository);
            var user2 = new LucidEntityBuilder().Save(_repository);

            var allUsers = _repository.Query<LucidEntity>().List();

            allUsers.Count.Should().Be(2);
            allUsers.Should().Contain(user1);
            allUsers.Should().Contain(user2);
        }

        [Test]
        public void Query_RestrictPropertyEqual()
        {
            var user1 = new LucidEntityBuilder().With(u => u.Email, "test1@user.net").Save(_repository);
            var user2 = new LucidEntityBuilder().With(u => u.Email, "test2@user.net").Save(_repository);

            var specificUser =
                _repository.Query<LucidEntity>()
                    .Filter(Where<LucidEntity>.PropEq(u => u.Email, "test2@user.net"))
                    .SingleOrDefault();

            specificUser.Should().NotBeNull();
            specificUser.Should().BeSameAs(user2);
        }
    }
}
