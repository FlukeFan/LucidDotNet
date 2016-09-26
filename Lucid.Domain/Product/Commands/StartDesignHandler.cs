using Lucid.Domain.Contract.Product.Commands;
using Lucid.Domain.Contract.Product.Responses;
using Lucid.Domain.Product.Responses;
using Lucid.Domain.Utility;

namespace Lucid.Domain.Product.Commands
{
    public class StartDesignHandler : HandleCommand<StartDesign, DesignDto>
    {
        public override DesignDto Execute(StartDesign cmd)
        {
            var design = Design.Start(cmd);
            return DesignDtoMapper.Map(design);
        }
    }
}
