using System;
using Lucid.Domain.Utility;
using NUnit.Framework;

namespace Lucid.Domain.Tests.Utility
{
    public abstract class DomainTest
    {
        protected TestValues Test;

        [SetUp]
        public virtual void SetUp()
        {
            Test = new TestValues();
        }

        protected DateTime SetDomainNow(DateTime now)
        {
            Registry.NowUtc = () => now;
            return now;
        }
    }
}
