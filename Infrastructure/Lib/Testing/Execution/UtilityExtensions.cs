using System;
using System.Linq.Expressions;
using Reposify.Testing;

namespace Lucid.Infrastructure.Lib.Testing.Execution
{
    public static class UtilityExtensions
    {
        public static TExecutable Mutate<TExecutable>(this TExecutable target, Action<TExecutable> mutator)
        {
            mutator(target);
            return target;
        }

        public static Builder<T> With<T, U>(this T target, Expression<Func<T, U>> propertyFunction, U value)
        {
            var builder = Builder.Modify(target);
            return builder.With(propertyFunction, value);
        }
    }
}
