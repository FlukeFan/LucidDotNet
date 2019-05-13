using System;

namespace Lucid.Lib.Facade.Pledge
{
    public interface IAgreement<TExecutable>
    {
        Func<TExecutable> Executable { get; }
    }

    public interface IAgreement<TExecutable, out TResult> : IAgreement<TExecutable>
    {
        Func<TResult> Result { get; }
    }

    public class Agreement<TExecutable> : IAgreement<TExecutable>
    {
        public Func<TExecutable> Executable { get; set; }
    }

    public class Agreement<TExecutable, TResult> : Agreement<TExecutable>, IAgreement<TExecutable, TResult>
    {
        public Func<TResult> Result { get; set; }
    }
}
