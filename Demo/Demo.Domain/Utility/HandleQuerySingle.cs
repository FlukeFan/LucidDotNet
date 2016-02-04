using System.Diagnostics;
using Lucid.Facade.Execution;

namespace Demo.Domain.Utility
{
    public abstract class HandleQuerySingle<TQuery, TReturn> : IHandleQuerySingle<TQuery, TReturn>
        where TQuery : IQuerySingle<TReturn>
    {
        protected static IDemoRepository Repository {[DebuggerStepThrough] get { return Registry.Repository; } }

        public abstract TReturn Find(TQuery query);
    }
}
