using System.Collections.Generic;
using System.Linq;
using Lucid.Domain.Contract.Product.Responses;

namespace Lucid.Domain.Product.Responses
{
    public static class DesignDtoMapper
    {
        public static DesignDto Map(Design design)
        {
            return new DesignDto
            {
                DesignId    = design.Id,
                Name        = design.Name,
            };
        }

        public static IList<DesignDto> Map(IEnumerable<Design> designs)
        {
            return designs.Select(d => Map(d)).ToList();
        }
    }
}
