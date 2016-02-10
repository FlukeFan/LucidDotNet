using System;
using FluentAssertions;
using Lucid.Persistence.Testing;
using NUnit.Framework;

namespace Lucid.Persistence.Tests.Testing
{
    public class MemoryRepositoryTests : IRepositoryTests
    {
        protected override IRepository<int> New()
        {
            return new MemoryRepository<int>(new ConsistencyInspector());
        }

        [Test]
        public virtual void Flush_DoesNotThrow()
        {
            New().Flush();
        }

        [Test]
        public void ShouldContain()
        {
            var repository = new MemoryRepository<int>(new ConsistencyInspector());
            var entity = new LucidPolyTypeBuilder().Value();

            repository.Save(entity);

            repository.ShouldContain(entity);
            repository.ShouldContain<LucidPolyType>(entity.Id);
        }

        [Test]
        public void ShouldContain_Throws()
        {
            var repository = new MemoryRepository<int>(new ConsistencyInspector());

            Assert.Throws<Exception>(() => repository.ShouldContain(null)).Message.Should().Contain("should not be null");
            Assert.Throws<Exception>(() => repository.ShouldContain(new LucidPolyTypeBuilder().Value())).Message.Should().Contain("has an unsaved Id value");
            Assert.Throws<Exception>(() => repository.ShouldContain(new LucidPolyTypeBuilder().With(e => e.Id, 1).Value())).Message.Should().Contain("Could not find");
            Assert.Throws<Exception>(() => repository.ShouldContain<LucidPolyType>(1)).Message.Should().Contain("Could not find");
        }
    }
}
