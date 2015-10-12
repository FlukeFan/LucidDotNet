using System.Collections.Generic;
using System.Linq;

namespace Lucid.Domain.Persistence.Queries
{
    public class Query<T, TId> where T : class, IEntity<TId>
    {
        private IRepository<TId>    _repository;
        private IList<Where<T>>     _restrictions = new List<Where<T>>();

        public IEnumerable<Where<T>> Restrictions { get { return _restrictions; } }

        public Query(IRepository<TId> repository)
        {
            _repository = repository;
        }

        public Query<T, TId> Filter(Where<T> restriction)
        {
            _restrictions.Add(restriction);
            return this;
        }

        public IList<T> List()
        {
            return _repository.Satisfy(this);
        }

        public T SingleOrDefault()
        {
            return List().SingleOrDefault();
        }
    }
}
