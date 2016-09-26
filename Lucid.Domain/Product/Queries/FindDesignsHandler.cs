using System.Collections.Generic;
using Demo.Domain.Contract.Product.Queries;
using Demo.Domain.Contract.Product.Responses;
using Demo.Domain.Product.Responses;
using Demo.Domain.Utility;
using Reposify.Queries;

namespace Demo.Domain.Product.Queries
{
    public class FindDesignsHandler : HandleQuery<FindDesigns, IList<DesignDto>>
    {
        public override IList<DesignDto> Find(FindDesigns query)
        {
            var designs =
                Repository.Query<Design>()
                    .OrderBy(d => d.Name, Direction.Ascending)
                    .List();

            return DesignDtoMapper.Map(designs);
        }
    }
}
