using SimpleFacade;

namespace Lucid.Domain.Utility
{
    public abstract class HandleQuery<TQuery, TReturn> : Handler, IHandleQuery<TQuery, TReturn>
        where TQuery : IQuery<TReturn>
    {
        public abstract TReturn Find(TQuery query);
    }
}
