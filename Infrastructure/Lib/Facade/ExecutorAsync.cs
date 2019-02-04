using System;
using System.Threading.Tasks;

namespace Lucid.Infrastructure.Lib.Facade
{
    public class ExecutorAsync : IExecutorAsync
    {
        Task<object> IExecutorAsync.ExecuteAsync(IExecutionContext context)
        {
            var iExecutable = (context.Executable as IExecutableAsync);

            if (iExecutable != null)
                return iExecutable.ExecuteAsync();

            throw new ArgumentException($"executable '{context.Executable}' did not implement IExecutable", "executable");
        }
    }
}
