using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Lucid.Lib.Facade;
using Reposify.Testing;

namespace Lucid.Lib.Testing.Execution
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

        public static async Task<TReturn> ExecAsync<TReturn>(this IExecutorAsync executorAsync, IQuery<TReturn> query)
        {
            var context = new ExecutionContext { Executable = query };
            return (TReturn)await executorAsync.ExecuteAsync(context);
        }
    }
}
