using System.Collections.Generic;
using Lucid.Domain.Contract.Product.Queries;
using Lucid.Domain.Contract.Product.Responses;
using Lucid.Domain.Product.Responses;
using Lucid.Domain.Utility;
using Reposify.Queries;

namespace Lucid.Domain.Product.Queries
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
