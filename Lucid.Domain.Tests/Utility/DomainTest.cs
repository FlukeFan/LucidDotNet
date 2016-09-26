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
            Registry.Repository = Repository;
        }

        [TearDown]
        public virtual void TearDown()
        {
            Registry.Repository = null;
            Test = null;
        }

        protected DateTime SetDomainNow(DateTime now)
        {
            Registry.NowUtc = () => now;
            return now;
        }
    }
}
