using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Lucid.Infrastructure.Lib.Facade;

namespace Lucid.Modules.AppFactory.Design
{
    public class FindBlueprintsQuery : QueryAsync<IList<Blueprint>>
    {
        public override Task<IList<Blueprint>> ExecuteAsync()
        {
            throw new NotImplementedException();
        }
    }
}
