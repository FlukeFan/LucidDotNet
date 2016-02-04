﻿using Demo.Domain.Utility;
using Lucid.Persistence.Testing;

namespace Demo.Domain.Tests.Utility
{
    public class DemoMemoryRepository : MemoryRepository<int>, IDemoRepository
    {
        public DemoMemoryRepository() : base(new DemoConsistencyInspector()) { }
    }
}
