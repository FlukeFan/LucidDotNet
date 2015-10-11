using System;
using Demo.Domain.Utility;
using Lucid.Domain.Testing;

namespace Demo.Domain.Tests.Utility
{
    public class DemoMemoryRepository : MemoryRepository<int>, IDemoRepository
    {
        private static DemoConsistencyInspector _validator = new DemoConsistencyInspector();

        public DemoMemoryRepository(Action<DemoEntity> beforeSave) : base(e => beforeSave((DemoEntity)e)) { }
    }
}
