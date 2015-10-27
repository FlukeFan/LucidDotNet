﻿using System;
using System.ComponentModel.DataAnnotations;
using Demo.Domain.Product.Responses;
using Demo.Domain.Utility;
using Lucid.Domain.Remote;

namespace Demo.Domain.Product.Commands
{
    public class StartDesign : Command<DesignDto>
    {
        [Required(ErrorMessage = "Please supply a Name")]
        [MaxLength(length: DemoEntity.DefaultMaxStringLength, ErrorMessage = "Name is too long")]
        public string Name { get; set; }

        public override DesignDto Execute()
        {
            throw new NotImplementedException();
        }
    }
}