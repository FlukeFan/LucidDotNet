using Demo.Web.Utility;
using Lucid.Domain.Execution;
using Lucid.Domain.Testing;

namespace Demo.Web.Tests.Utility
{
    public static class WebTestRegistry
    {
        public static ExecutorStub ExecutorStub;

        public static void SetupExecutorStub()
        {
            ExecutorStub = new ExecutorStub();
            PresentationRegistry.Executor = new CqExecutor(ExecutorStub);
        }
    }
}
