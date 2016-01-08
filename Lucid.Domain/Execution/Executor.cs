using System;

namespace Lucid.Domain.Execution
{
    public class Executor : IExecutor
    {
        public object Execute(object executable)
        {
            var iExecutable = executable as IExecutable;

            if (iExecutable != null)
                return iExecutable.Execute();

            return UnhandledExecute(executable);
        }

        public virtual object UnhandledExecute(object executable)
        {
            throw new Exception("Could not execute ");
        }
    }
}
