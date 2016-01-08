using Demo.Infrastructure.NHibernate;
using Lucid.Domain.Execution;

namespace Demo.Web.Utility
{
    public class PresentationRegistry
    {
        public static MvcExecutor Executor =
            new MvcExecutor(
                new RepositoryExecutor(
                    new ValidatingExecutor(
                        new Executor()
                    )
                )
            );
    }
}