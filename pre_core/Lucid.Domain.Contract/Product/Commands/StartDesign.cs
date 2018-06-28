using System.ComponentModel.DataAnnotations;
using Lucid.Domain.Contract.Product.Responses;
using SimpleFacade;

namespace Lucid.Domain.Contract.Product.Commands
{
    public class StartDesign : ICommand<DesignDto>
    {
        [Required(ErrorMessage = "Please supply a Name")]
        [MaxLength(length: Constraints.DefaultMaxStringLength, ErrorMessage = "Name is too long")]
        public string Name { get; set; }
    }
}
