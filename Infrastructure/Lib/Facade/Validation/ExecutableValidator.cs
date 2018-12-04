using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Lucid.Infrastructure.Lib.Facade.Exceptions;

namespace Lucid.Infrastructure.Lib.Facade.Validation
{
    public static class ExecutableValidator
    {
        public static void Validate(object dto)
        {
            var context = new ValidationContext(dto);
            var results = new List<ValidationResult>();
            var valid = Validator.TryValidateObject(dto, context, results, true);

            if (!valid)
                throw new FacadeException(results);

            var customValidation = dto as ICustomValidation;

            if (customValidation != null)
                customValidation.Validate();
        }
    }
}
