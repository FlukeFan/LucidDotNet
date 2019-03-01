namespace Lucid.Infrastructure.Lib.Testing.Execution
{
    public class AgreementBuilder
    {
        public static AgreementBuilder<TExecutable> For<TExecutable>(TExecutable executable)
        {
            return new AgreementBuilder<TExecutable> { Executable = executable };
        }
    }

    public class AgreementBuilder<TExecutable>
    {
        public TExecutable Executable;

        public Agreement<TExecutable, TResult> Result<TResult>(TResult result)
        {
            return new Agreement<TExecutable, TResult>
            {
                Executable = Executable,
                Result = result,
            };
        }
    }
}
