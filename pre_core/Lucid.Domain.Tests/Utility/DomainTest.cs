using System;
using Lucid.Domain.Utility;
using NUnit.Framework;

namespace Lucid.Domain.Tests.Utility
{
    [TestFixture]
    public abstract class DomainTest
    {
        protected TestValues            Test        { get; set; }
        protected LucidMemoryRepository  Repository  { get; set; }

        [SetUp]
        public virtual void SetUp()
        {
            Test = new TestValues();
            SetDomainNow(DateTime.UtcNow);

            Repository = new LucidMemoryRepository();
            DomainRegistry.Repository = Repository;
        }

        [TearDown]
        public virtual void TearDown()
        {
            DomainRegistry.Repository = null;
            Test = null;
        }

        protected DateTime SetDomainNow(DateTime now)
        {
            DomainRegistry.NowUtc = () => now;
            return now;
        }
    }
}
