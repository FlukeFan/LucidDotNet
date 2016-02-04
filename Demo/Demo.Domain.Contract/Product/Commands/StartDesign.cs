using System.ComponentModel.DataAnnotations;
using Demo.Domain.Contract.Product.Responses;
using Lucid.Facade.Execution;

namespace Demo.Domain.Contract.Product.Commands
{
    public class StartDesign : ICommand<DesignDto>
    {
        [Required(ErrorMessage = "Please supply a Name")]
        [MaxLength(length: Constraints.DefaultMaxStringLength, ErrorMessage = "Name is too long")]
        public string Name { get; set; }
    }
}
