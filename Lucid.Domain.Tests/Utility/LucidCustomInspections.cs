using System;
using Reposify.Testing;

namespace Lucid.Domain.Tests.Utility
{
    public static class LucidCustomInspections
    {
        public static void Add<T>(Action<LucidConsistencyInspector, T> inspection)
        {
            CustomInspections.Add<T>((ci, entity) => inspection((LucidConsistencyInspector)ci, entity));
        }
    }
}
