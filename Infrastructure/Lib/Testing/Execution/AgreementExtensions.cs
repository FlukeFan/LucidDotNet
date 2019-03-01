using FluentAssertions;

namespace Lucid.Infrastructure.Lib.Testing.Execution
{
    public static class AgreementExtensions
    {
        public static void StubResult<TExecutable, TResult>(this ExecutorStubAsync executorStub, Agreement<TExecutable, TResult> agreement)
        {
            executorStub.StubResult<TExecutable>(agreement.Result());
        }

        public static void VerifySingleExecuted<TExecutable, TResult>(this ExecutorStubAsync executorStub, Agreement<TExecutable, TResult> agreement)
        {
            executorStub.SingleExecuted<TExecutable>().Should().BeEquivalentTo(agreement.Executable());
        }
    }
}
