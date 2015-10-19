using FluentAssertions;
using Lucid.Domain.Persistence;
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
        public virtual void Save_SetsId()
        {
            var user = new LucidPolyTypeBuilder().Value();

            user.Id.Should().Be(0, "newly instantiated entity should have a default Id value");

            var savedUser = _repository.Save(user);

            savedUser.Should().BeSameAs(user);
            savedUser.Id.Should().NotBe(0, "persisted entity should have a non-zero Id");
        }

        [Test]
        public virtual void Query_RetrieveAll()
        {
            var user1 = new LucidPolyTypeBuilder().Save(_repository);
            var user2 = new LucidPolyTypeBuilder().Save(_repository);

            var allUsers = _repository.Query<LucidPolyType>().List();

            allUsers.Count.Should().Be(2);
            allUsers.Should().Contain(user1);
            allUsers.Should().Contain(user2);
        }

        [Test]
        public virtual void Query_RestrictPropertyEqual()
        {
            var user1 = new LucidPolyTypeBuilder().With(u => u.String, "test1@user.net").Save(_repository);
            var user2 = new LucidPolyTypeBuilder().With(u => u.String, "test2@user.net").Save(_repository);

            var specificUser =
                _repository.Query<LucidPolyType>()
                    .Filter(e => e.String == "test2@user.net")
                    .SingleOrDefault();

            specificUser.Should().NotBeNull();
            specificUser.Should().BeSameAs(user2);
        }
    }
}
