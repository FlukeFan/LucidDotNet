using System;
using System.Threading.Tasks;
using Lucid.Infrastructure.Lib.Facade;
using NHibernate;

namespace Lucid.Infrastructure.Lib.Domain.SqlServer
{
    public class RepositoryExecutorAsync : IExecutorAsync
    {
        private ISessionFactory             _sessionFactory;
        private Func<INhSqlRepository>      _accessor;
        private Action<INhSqlRepository>    _mutator;
        private IExecutorAsync              _inner;

        public RepositoryExecutorAsync(ISessionFactory sessionFactory, Func<INhSqlRepository> accessor, Action<INhSqlRepository> mutator,  IExecutorAsync inner)
        {
            _sessionFactory = sessionFactory;
            _accessor = accessor;
            _mutator = mutator;
            _inner = inner;
        }

        async Task<object> IExecutorAsync.ExecuteAsync(IExecutionContext context)
        {
            using (var session = _sessionFactory.OpenSession())
            using (var repository = new NhSqlRepository(session))
            {
                var previous = _accessor();
                try
                {
                    repository.BeginTransaction();
                    _mutator(repository);
                    var result = await _inner.ExecuteAsync(context);
                    repository.Commit();
                    return result;
                }
                finally
                {
                    _mutator(previous);
                }
            }
        }
    }
}
