using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lucid.Infrastructure.Lib.Facade;

namespace Lucid.Infrastructure.Lib.Testing.Execution
{
    public class ExecutorStub : IExecutor
    {
        private IDictionary<Type, object> _stubResults = new Dictionary<Type, object>();

        public IList<object> AllExecuted = new List<object>();

        public IList<T> Executed<T>()
        {
            return AllExecuted
                .Where(e => typeof(T).IsAssignableFrom(e.GetType()))
                .Cast<T>()
                .ToList();
        }

        public T SingleExecuted<T>()
        {
            var executed = Executed<T>();

            if (executed.Count != 1)
                throw new Exception($"Expected 1 execution of {typeof(T)} but got {executed.Count}");

            return executed[0];
        }

        public void StubResult<T>(object result)
        {
            _stubResults[typeof(T)] = result;
        }

        Task<object> IExecutor.Execute(object executable)
        {
            if (executable == null)
                throw new Exception("Null executable passed to IExecutor.Execute");

            AllExecuted.Add(executable);

            return Task.FromResult(_stubResults.ContainsKey(executable.GetType())
                ? _stubResults[executable.GetType()]
                : null);
        }
    }
}
