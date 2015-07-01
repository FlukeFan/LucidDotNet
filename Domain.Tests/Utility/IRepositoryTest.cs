using FluentAssertions;
using Lucid.Domain.Tests.Orgs;
using Lucid.Domain.Utility;
using NUnit.Framework;

namespace Lucid.Domain.Tests.Utility
{
    [TestFixture]
    public abstract class IRepositoryTest
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
    }
}
