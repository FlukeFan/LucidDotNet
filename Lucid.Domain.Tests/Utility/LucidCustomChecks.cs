using System;
using Reposify.Testing;

namespace Lucid.Domain.Tests.Utility
{
    public static class LucidCustomChecks
    {
        public static void Add<T>(Action<LucidConstraintChecker, T> inspection)
        {
            CustomChecks.Add<T>((ci, entity) => inspection((LucidConstraintChecker)ci, entity));
        }
    }
}
