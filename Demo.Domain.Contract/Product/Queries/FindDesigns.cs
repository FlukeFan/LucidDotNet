using System.Collections.Generic;
using Demo.Domain.Contract.Product.Responses;
using Lucid.Facade.Execution;

namespace Demo.Domain.Contract.Product.Queries
{
    public class FindDesigns : IQuery<IList<DesignDto>>
    {
    }
}
