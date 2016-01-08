using System.Collections.Generic;

namespace Lucid.Domain.Execution
{
    public abstract class QueryList<T> : IExecutable
    {
        public abstract IList<T> List();

        object IExecutable.Execute()
        {
            return List();
        }
    }
}
