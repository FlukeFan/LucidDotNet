using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Lucid.Infrastructure.Lib.Facade;

namespace Lucid.Infrastructure.Lib.Testing.Execution
{
    public class ExecutorStub : IExecutor
    {
        private IDictionary<Type, object> _stubResults = new Dictionary<Type, object>();

        public IList<object> Executed = new List<object>();

        Task<object> IExecutor.Execute(IExecutable executable)
        {
            Executed.Add(executable);

            return Task.FromResult(_stubResults.ContainsKey(executable.GetType())
                ? _stubResults[executable.GetType()]
                : null);
        }

        public void StubResult<T>(object result)
        {
            _stubResults[typeof(T)] = result;
        }
    }
}
