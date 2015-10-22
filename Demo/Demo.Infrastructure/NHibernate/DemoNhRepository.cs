using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Demo.Domain.Utility;
using Lucid.Infrastructure.Persistence.NHibernate;

namespace Demo.Infrastructure.NHibernate
{
    public class DemoNhRepository : NhRepository<int>, IDemoRepository
    {
    }
}
