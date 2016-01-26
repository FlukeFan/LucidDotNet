using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Lucid.Domain.Exceptions;

namespace Lucid.Domain.Validation
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

            var errorContext = CreateErrorContext(results);
            throw new LucidException(errorContext);
        }

        public static ErrorContext CreateErrorContext(IEnumerable<ValidationResult> results)
        {
            var error = new ErrorContext { IsLogicException = true };

            foreach (var result in results)
                error.Add(result);

            return error;
        }
    }
}
