using System.Collections.Generic;

namespace Lucid.Domain.Utility
{
    public interface IRemoteable
    {
        object Execute();
    }

    public abstract class Command<T> : IRemoteable
    {
        public abstract T Execute();

        object IRemoteable.Execute()
        {
            return Execute();
        }
    }

    public abstract class CommandVoid : IRemoteable
    {
        public abstract void Execute();

        object IRemoteable.Execute()
        {
            Execute();
            return null;
        }
    }

    public abstract class QueryList<T> : IRemoteable
    {
        public abstract IList<T> Execute();

        object IRemoteable.Execute()
        {
            return Execute();
        }
    }

    public abstract class QuerySingle<T> : IRemoteable
    {
        public abstract T Execute();

        object IRemoteable.Execute()
        {
            return Execute();
        }
    }
}
