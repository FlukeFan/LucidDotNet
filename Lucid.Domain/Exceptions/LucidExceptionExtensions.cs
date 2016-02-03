using System;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;

namespace Lucid.Domain.Exceptions
{
    public static class LucidExceptionExtensions
    {
        public static LucidException PropertyError<T>(this T source, Expression<Func<T, object>> property, Func<string, string> message)
        {
            var propertyName = ((MemberExpression)property.Body).Member.Name;
            var validationResult = new ValidationResult(message(propertyName), new string[] { propertyName });
            return new LucidException(new ValidationResult[] { validationResult });
        }
    }
}
