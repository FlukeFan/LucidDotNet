using System.Collections.Generic;
using System.Linq;

namespace Lucid.Domain.Utility.Queries
{
    public class Query<T>
    {
        private IRepository _repository;

        public Query(IRepository repository)
        {
            _repository = repository;
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
