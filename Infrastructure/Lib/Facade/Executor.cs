using System;
using System.Threading.Tasks;

namespace Lucid.Infrastructure.Lib.Facade
{
    public class Executor : IExecutor
    {
        Task<object> IExecutor.Execute(object executable)
        {
            var iExecutable = (executable as IExecutable);

            if (iExecutable != null)
                return iExecutable.Execute();

            throw new ArgumentException($"executable '{executable}' did not implement IExecutable", "executable");
        }
    }
}
