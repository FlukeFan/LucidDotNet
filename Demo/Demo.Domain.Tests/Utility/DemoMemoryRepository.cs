using System;
using Demo.Domain.Utility;
using Lucid.Domain.Testing;

namespace Lucid.Domain.Tests.Utility
{
    public class DemoMemoryRepository : MemoryRepository<int>, IDemoRepository
    {
        public DemoMemoryRepository(Action<DemoEntity> beforeSave) : base(e => beforeSave((DemoEntity)e)) { }
    }
}
