using System.Collections.Generic;
using Demo.Domain.Contract.Product.Queries;
using Demo.Domain.Contract.Product.Responses;
using Demo.Domain.Product.Responses;
using Demo.Domain.Utility;
using Lucid.Persistence.Queries;

namespace Demo.Domain.Product.Queries
{
    public class FindDesignsHandler : HandleQueryList<FindDesigns, DesignDto>
    {
        public override IList<DesignDto> List(FindDesigns query)
        {
            var designs =
                Repository.Query<Design>()
                    .OrderBy(d => d.Name, Direction.Ascending)
                    .List();

            return DesignDtoMapper.Map(designs);
        }
    }
}
