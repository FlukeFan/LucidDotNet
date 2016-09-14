using System.Diagnostics;
using Lucid.Facade.Execution;

namespace Demo.Domain.Utility
{
    public abstract class HandleQuery<TQuery, TReturn> : IHandleQuery<TQuery, TReturn>
        where TQuery : IQuery<TReturn>
    {
        protected static IDemoRepository Repository {[DebuggerStepThrough] get { return Registry.Repository; } }

        public abstract TReturn Find(TQuery query);
    }
}
