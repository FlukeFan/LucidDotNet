using System.Collections.Generic;

namespace Lucid.Domain.Remote
{
    public abstract class QueryList<T> : IRemoteable
    {
        public abstract IList<T> Execute();

        object IRemoteable.Execute()
        {
            return Execute();
        }
    }
}
