using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Lucid.Domain.Utility
{
    public static class Validator
    {
        public static void Validate(object remoteable)
        {
            var type = remoteable.GetType();
            foreach (var property in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                if (!property.CanWrite)
                    continue;

                var value = property.GetValue(remoteable);

                foreach (ValidationAttribute annotation in property.GetCustomAttributes(typeof(ValidationAttribute), true))
                    if (!annotation.IsValid(value))
                        throw new Exception(property.Name + " has invalid value " + annotation.FormatErrorMessage(property.Name));
            }
        }
    }
}
