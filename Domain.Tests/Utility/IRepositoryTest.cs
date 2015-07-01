using FluentAssertions;
using Lucid.Domain.Orgs;
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
            var user = (User)null;

            var savedUser = repository.Save(user);

            savedUser.Should().BeSameAs(user);
            savedUser.Id.Should().Be(-123, "wip");
        }
    }
}
