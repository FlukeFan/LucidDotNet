using Lucid.Web.Utility;
using SimpleFacade.Execution;
using SimpleFacade.Testing;

namespace Lucid.Web.Tests.Utility
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
