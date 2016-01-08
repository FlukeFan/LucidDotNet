using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Lucid.Domain.Execution
{
    public static class Validator
    {
        public static void Validate(object executable)
        {
            var type = executable.GetType();
            foreach (var property in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                if (!property.CanWrite)
                    continue;

                var value = property.GetValue(executable);

                foreach (ValidationAttribute annotation in property.GetCustomAttributes(typeof(ValidationAttribute), true))
                    if (!annotation.IsValid(value))
                        throw new Exception(property.Name + " has invalid value " + annotation.FormatErrorMessage(property.Name));
            }
        }
    }
}
