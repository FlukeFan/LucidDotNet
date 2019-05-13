using System;
using Lucid.Lib.Facade.Exceptions;
using Lucid.Lib.Facade.Validation;
using NUnit.Framework;

namespace Lucid.Lib.Testing.Validation
{
    public static class ValidationExtensions
    {
        public static void ShouldBeValid<T>(this T command)
        {
            ExecutableValidator.Validate(command);
        }

        public static void ShouldBeInvalid<T>(this T command, Action<T> invalidate, string reason)
        {
            invalidate(command);
            Assert.Throws<FacadeException>(() =>
            {
                ExecutableValidator.Validate(command);
            }, reason);
        }
    }
}
