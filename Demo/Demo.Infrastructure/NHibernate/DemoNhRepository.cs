using Demo.Domain.Utility;
using Lucid.Infrastructure.Persistence.NHibernate;

namespace Demo.Infrastructure.NHibernate
{
    public class DemoNhRepository : NhRepository<int>, IDemoRepository
    {
    }
}
