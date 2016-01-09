using Demo.Domain.Contract.Product.Commands;
using Demo.Domain.Contract.Product.Responses;
using Demo.Domain.Product.Responses;
using Lucid.Domain.Execution;

namespace Demo.Domain.Product.Commands
{
    public class StartDesignHandler : IHandleCommand<StartDesign, DesignDto>
    {
        public DesignDto Execute(StartDesign cmd)
        {
            var design = Design.Start(cmd);
            return DesignDtoMapper.Map(design);
        }
    }
}
