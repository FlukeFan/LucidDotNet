using System;

namespace Lucid.Infrastructure.Lib.Testing.Execution
{
    public class Agreement<TExecutable, TResult>
    {
        public Func<TExecutable>    Executable;
        public Func<TResult>        Result;
    }
}
