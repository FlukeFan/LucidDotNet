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

        public IEnumerable<Where>   Restrictions    { get { return _restrictions; } }

        public Query(IRepository<TId> repository)
        {
            _repository = repository;
        }

        public Query<T, TId> Filter(Expression<Func<T, bool>> restriction)
        {
            _restrictions.Add(Where.For(restriction));
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
