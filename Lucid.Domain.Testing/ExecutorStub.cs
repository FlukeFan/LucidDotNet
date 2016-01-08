using System.Collections.Generic;
using Lucid.Domain.Execution;

namespace Lucid.Domain.Testing
{
    public class ExecutorStub : IExecutor
    {
        private IList<object> _executed = new List<object>();

        public IEnumerable<object> Executed { get { return _executed; } }

        public object Execute(object executable)
        {
            _executed.Add(executable);
            return null;
        }
    }
}
