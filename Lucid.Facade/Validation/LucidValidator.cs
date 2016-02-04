using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Lucid.Facade.Exceptions;

namespace Lucid.Facade.Validation
{
    public static class LucidValidator
    {
        public static void Validate(object dto)
        {
            var context = new ValidationContext(dto);
            var results = new List<ValidationResult>();
            var valid = Validator.TryValidateObject(dto, context, results, true);

            if (valid)
                return;

            throw new LucidException(results);
        }
    }
}
