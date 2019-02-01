using System;
using Lucid.Infrastructure.Lib.Facade;
using Lucid.Infrastructure.Lib.Testing.Execution;

namespace Lucid.Infrastructure.Lib.Testing.Controller
{
    public class SetupExecutorStub : IDisposable
    {
        private IExecutor           _previous;
        private Action<IExecutor>   _onDispose;

        public SetupExecutorStub(ref IExecutor executor, Action<IExecutor> onDispose)
        {
            _previous = executor;
            _onDispose = onDispose;
            executor = Stub = new ExecutorStub();
        }

        public ExecutorStub Stub { get; private set; }

        public void Dispose()
        {
            _onDispose(_previous);
        }
    }
}
