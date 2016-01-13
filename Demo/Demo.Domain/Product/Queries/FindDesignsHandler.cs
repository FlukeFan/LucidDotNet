using System.Collections.Generic;
using Demo.Domain.Contract.Product.Queries;
using Demo.Domain.Contract.Product.Responses;
using Demo.Domain.Product.Responses;
using Demo.Domain.Utility;

namespace Demo.Domain.Product.Queries
{
    public class FindDesignsHandler : HandleQueryList<FindDesigns, DesignDto>
    {
        public override IList<DesignDto> List(FindDesigns query)
        {
            var designs =
                Repository.Query<Design>()
                    .List();

            return DesignDtoMapper.Map(designs);
        }
    }
}
