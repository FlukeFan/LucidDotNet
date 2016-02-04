using System;
using Lucid.Persistence.Testing;

namespace Demo.Domain.Tests.Utility
{
    public static class DemoCustomInspections
    {
        public static void Add<T>(Action<DemoConsistencyInspector, T> inspection)
        {
            CustomInspections.Add<T>((ci, entity) => inspection((DemoConsistencyInspector)ci, entity));
        }
    }
}
