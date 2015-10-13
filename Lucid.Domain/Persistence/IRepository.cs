using System.Collections.Generic;
using Lucid.Domain.Persistence.Queries;

namespace Lucid.Domain.Persistence
{
    public interface IRepository<TId>
    {
        T               Save<T>(T entity)               where T : IEntity<TId>;

        Query<T, TId>   Query<T>()                      where T : IEntity<TId>;
        IList<T>        Satisfy<T>(Query<T, TId> query) where T : IEntity<TId>;
    }
}
