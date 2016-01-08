using System.Collections.Generic;
using System.Linq;
using Lucid.Domain.Execution;

namespace Lucid.Domain.Testing
{
    public class ExecutorStub : IExecutor
    {
        private IList<object> _executed = new List<object>();

        public IEnumerable<object> AllExecuted()
        {
            return _executed;
        }

        public IEnumerable<T> Executed<T>()
        {
            return _executed.Where(e => typeof(T).IsAssignableFrom(e.GetType())).Cast<T>();
        }

        public object Execute(object executable)
        {
            _executed.Add(executable);
            return null;
        }
    }
}
