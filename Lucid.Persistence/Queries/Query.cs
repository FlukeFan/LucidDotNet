using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Lucid.Persistence.Queries
{
    public class Query<T, TId> where T : IEntity<TId>
    {
        private IRepository<TId>    _repository;
        private IList<Where>        _restrictions   = new List<Where>();
        private IList<Ordering>     _orders         = new List<Ordering>();

        public IEnumerable<Where>       Restrictions    { get { return _restrictions; } }
        public IEnumerable<Ordering>    Orders          { get { return _orders; } }

        public Query(IRepository<TId> repository)
        {
            _repository = repository;
        }

        public Query<T, TId> Filter(Expression<Func<T, bool>> restriction)
        {
            _restrictions.Add(Where.For(restriction));
            return this;
        }

        public Query<T, TId> OrderBy<TKey>(Expression<Func<T, TKey>> property, Direction direction)
        {
            _orders.Add(Ordering.For(property, direction));
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
