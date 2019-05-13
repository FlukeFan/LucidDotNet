using System;
using Lucid.Lib.Facade;
using Lucid.Lib.Testing.Execution;

namespace Lucid.Lib.Testing.Controller
{
    public class SetupExecutorStub : IDisposable
    {
        private IExecutorAsync          _previous;
        private Action<IExecutorAsync>  _onDispose;

        public SetupExecutorStub(IExecutorAsync current, Action<IExecutorAsync> setup)
        {
            _previous = current;
            _onDispose = setup;
            Stub = new ExecutorStubAsync();
            setup(Stub);
        }

        public ExecutorStubAsync Stub { get; private set; }

        public void Dispose()
        {
            _onDispose(_previous);
        }
    }
}
