using System;
using Lucid.Infrastructure.Lib.Facade;
using Lucid.Infrastructure.Lib.Testing.Execution;

namespace Lucid.Infrastructure.Lib.Testing.Controller
{
    public class SetupExecutorStub : IDisposable
    {
        private IExecutor           _previous;
        private Action<IExecutor>   _onDispose;

        public SetupExecutorStub(IExecutor current, Action<IExecutor> setup)
        {
            _previous = current;
            _onDispose = setup;
            Stub = new ExecutorStub();
            setup(Stub);
        }

        public ExecutorStub Stub { get; private set; }

        public void Dispose()
        {
            _onDispose(_previous);
        }
    }
}
