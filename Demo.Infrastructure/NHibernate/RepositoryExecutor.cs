using Demo.Domain.Utility;
using SimpleFacade;

namespace Demo.Infrastructure.NHibernate
{
    public class RepositoryExecutor : IExecutor
    {
        private IExecutor _inner;

        public RepositoryExecutor(IExecutor inner)
        {
            _inner = inner;
        }

        public object Execute(object executable)
        {
            using (var repository = new DemoNhRepository())
            {
                repository.Open();
                Registry.Repository = repository;
                var response = _inner.Execute(executable);
                repository.Commit();
                Registry.Repository = null;
                return response;
            }
        }
    }
}
