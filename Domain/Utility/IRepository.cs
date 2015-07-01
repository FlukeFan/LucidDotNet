
using System.Collections.Generic;
using Lucid.Domain.Utility.Queries;
namespace Lucid.Domain.Utility
{
    public interface IRepository
    {
        T           Save<T>(T entity)   where T : Entity;

        Query<T>    Query<T>()          where T : Entity;
        IList<T>    Satisfy<T>(Query<T> query);
    }
}
