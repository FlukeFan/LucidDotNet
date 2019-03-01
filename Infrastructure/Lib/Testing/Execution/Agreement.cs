using System;

namespace Lucid.Infrastructure.Lib.Testing.Execution
{
    public class Agreement<TExecutable>
    {
        public Func<TExecutable> Executable;
    }

    public class Agreement<TExecutable, TResult> : Agreement<TExecutable>
    {
        public Func<TResult> Result;
    }
}
