using System;
using System.ComponentModel.DataAnnotations;
using Demo.Domain.Product.Responses;
using Lucid.Domain.Remote;

namespace Demo.Domain.Product.Commands
{
    public class StartDesign : Command<DesignDto>
    {
        [Required(ErrorMessage = "Please supply a Name")]
        [MaxLength(length: 255, ErrorMessage = "Name is too long")]
        public string Name { get; set; }

        public override DesignDto Execute()
        {
            throw new NotImplementedException();
        }
    }
}
