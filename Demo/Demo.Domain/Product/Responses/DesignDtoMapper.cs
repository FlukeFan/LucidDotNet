using Demo.Domain.Contract.Product.Responses;

namespace Demo.Domain.Product.Responses
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
    }
}
