﻿using Demo.Domain.Contract.Product.Commands;
using Demo.Domain.Contract.Product.Responses;
using Demo.Domain.Product.Responses;
using Demo.Domain.Utility;

namespace Demo.Domain.Product.Commands
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