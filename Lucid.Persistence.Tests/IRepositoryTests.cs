using System.Linq;
using FluentAssertions;
using Lucid.Persistence.Queries;
using NUnit.Framework;

namespace Lucid.Persistence.Tests
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
            var poly = new LucidPolyTypeBuilder().Value();

            poly.Id.Should().Be(0, "newly instantiated entity should have a default Id value");

            var savedUser = _repository.Save(poly);

            savedUser.Should().BeSameAs(poly);
            savedUser.Id.Should().NotBe(0, "persisted entity should have a non-zero Id");
        }

        [Test]
        public virtual void Load_RetrievesSavedObject()
        {
            var poly = new LucidPolyTypeBuilder().Value();

            _repository.Save(poly);
            poly.Id.Should().NotBe(0);

            var loaded = _repository.Load<LucidPolyType>(poly.Id);
            loaded.Should().BeSameAs(poly);
        }

        [Test]
        public virtual void Load_IdentityMapEnsuresReferencesEqual()
        {
            var poly = new LucidPolyTypeBuilder().Value();

            _repository.Save(poly);
            poly.Id.Should().NotBe(0);

            var loaded1 = _repository.Load<LucidPolyType>(poly.Id);
            var loaded2 = _repository.Load<LucidPolyType>(poly.Id);

            loaded1.Should().BeSameAs(loaded2);
        }

        [Test]
        public virtual void Query_RetrieveAll()
        {
            var poly1 = new LucidPolyTypeBuilder().Save(_repository);
            var poly2 = new LucidPolyTypeBuilder().Save(_repository);

            var allUsers = _repository.Query<LucidPolyType>().List();

            allUsers.Count.Should().Be(2);
            allUsers.Should().Contain(poly1);
            allUsers.Should().Contain(poly2);
        }

        [Test]
        public virtual void Query_RestrictStringPropertyEqual()
        {
            var poly1 = new LucidPolyTypeBuilder().With(u => u.String, "test1@user.net").Save(_repository);
            var poly2 = new LucidPolyTypeBuilder().With(u => u.String, "test2@user.net").Save(_repository);

            var specificUser =
                _repository.Query<LucidPolyType>()
                    .Filter(e => e.String == "test2@user.net")
                    .SingleOrDefault();

            specificUser.Should().NotBeNull();
            specificUser.Should().BeSameAs(poly2);
        }

        [Test]
        public virtual void Query_RestrictComparisons()
        {
            var poly1 = new LucidPolyTypeBuilder().With(u => u.Int, 1).Save(_repository);
            var poly2 = new LucidPolyTypeBuilder().With(u => u.Int, 2).Save(_repository);
            var poly3 = new LucidPolyTypeBuilder().With(u => u.Int, 3).Save(_repository);

            {
                var result = _repository.Query<LucidPolyType>().Filter(e => e.Int < 2).List();
                result.Count.Should().Be(1);
            }
            {
                var result = _repository.Query<LucidPolyType>().Filter(e => e.Int <= 2).List();
                result.Count.Should().Be(2);
            }
            {
                var result = _repository.Query<LucidPolyType>().Filter(e => e.Int == 2).List();
                result.Count.Should().Be(1);
            }
            {
                var result = _repository.Query<LucidPolyType>().Filter(e => e.Int > 2).List();
                result.Count.Should().Be(1);
            }
            {
                var result = _repository.Query<LucidPolyType>().Filter(e => e.Int >= 2).List();
                result.Count.Should().Be(2);
            }
        }

        [Test]
        public virtual void Query_Ordering()
        {
            var poly2 = new LucidPolyTypeBuilder().With(u => u.Int, 2).Save(_repository);
            var poly1 = new LucidPolyTypeBuilder().With(u => u.Int, 1).Save(_repository);
            var poly3 = new LucidPolyTypeBuilder().With(u => u.Int, 3).Save(_repository);

            {
                var result = _repository.Query<LucidPolyType>().OrderBy(e => e.Int, Direction.Ascending).List();
                result.Select(e => e.Int).Should().BeInAscendingOrder(e => e);
            }
            {
                var result = _repository.Query<LucidPolyType>().OrderBy(e => e.Int, Direction.Descending).List();
                result.Select(e => e.Int).Should().BeInDescendingOrder(e => e);
            }
        }

        [Test]
        public virtual void Query_Paging()
        {
            var poly1 = new LucidPolyTypeBuilder().With(u => u.Int, 1).Save(_repository);
            var poly2 = new LucidPolyTypeBuilder().With(u => u.Int, 2).Save(_repository);
            var poly3 = new LucidPolyTypeBuilder().With(u => u.Int, 3).Save(_repository);
            var poly4 = new LucidPolyTypeBuilder().With(u => u.Int, 4).Save(_repository);

            var result = _repository.Query<LucidPolyType>()
                .OrderBy(e => e.Int, Direction.Ascending)
                .Skip(1)
                .Take(2)
                .List();

            result.Select(e => e.Int).Should().BeEquivalentTo(2, 3);
        }
    }
}
