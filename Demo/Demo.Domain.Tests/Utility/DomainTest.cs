﻿using System;
using Demo.Domain.Utility;
using NUnit.Framework;

namespace Lucid.Domain.Tests.Utility
{
    public abstract class DomainTest
    {
        protected TestValues            Test;
        protected DemoMemoryRepository  Repository;

        [SetUp]
        public virtual void SetUp()
        {
            Test = new TestValues();
            
            Repository = new DemoMemoryRepository(LucidPersistenceValidator.BeforeSave);
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
