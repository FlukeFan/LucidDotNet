using System;
using Demo.Domain.Utility;
using NUnit.Framework;

namespace Demo.Domain.Tests.Utility
{
    [TestFixture]
    public abstract class DomainTest
    {
        protected TestValues            Test        { get; set; }
        protected DemoMemoryRepository  Repository  { get; set; }

        [SetUp]
        public virtual void SetUp()
        {
            Test = new TestValues();

            Repository = new DemoMemoryRepository();
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
