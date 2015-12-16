using Demo.Domain.Utility;
using Lucid.Persistence.NHibernate;

namespace Demo.Infrastructure.NHibernate
{
    public class DemoNhRepository : NhRepository<int>, IDemoRepository
    {
    }
}
