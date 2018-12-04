using System;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;

namespace Lucid.Infrastructure.Lib.Facade.Exceptions
{
    public static class FacadeExceptionExtensions
    {
        public static FacadeException PropertyException<T>(this T source, Expression<Func<T, object>> property, Func<string, string> message)
        {
            var propertyName = ((MemberExpression)property.Body).Member.Name;
            var validationResult = new ValidationResult(message(propertyName), new string[] { propertyName });
            return new FacadeException(new ValidationResult[] { validationResult });
        }
    }
}
