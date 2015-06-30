using System;
using Lucid.Domain.Utility;
using NUnit.Framework;

namespace Lucid.Domain.Tests.Utility
{
    public abstract class DomainTest
    {
        protected TestValues        Test;
        protected MemoryRepository  Repository;

        [SetUp]
        public virtual void SetUp()
        {
            Test = new TestValues();
            
            Repository = new MemoryRepository();
            Registry.Repository = Repository;
        }

        protected DateTime SetDomainNow(DateTime now)
        {
            Registry.NowUtc = () => now;
            return now;
        }
    }
}
