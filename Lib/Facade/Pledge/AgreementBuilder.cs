using System;

namespace Lucid.Lib.Facade.Pledge
{
    public class AgreementBuilder
    {
        public static AgreementBuilder<TExecutable> For<TExecutable>(Func<TExecutable> executable)
        {
            return new AgreementBuilder<TExecutable> { Executable = executable };
        }
    }

    public class AgreementBuilder<TExecutable>
    {
        public Func<TExecutable> Executable;

        public Agreement<TExecutable, TResult> Result<TResult>(Func<TResult> result)
        {
            return new Agreement<TExecutable, TResult>
            {
                Executable = Executable,
                Result = result,
            };
        }

        public Agreement<TExecutable> NoResultDefined()
        {
            return new Agreement<TExecutable>
            {
                Executable = Executable,
            };
        }
    }
}
