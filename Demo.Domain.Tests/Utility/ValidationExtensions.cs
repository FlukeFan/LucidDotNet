using System;
using NUnit.Framework;
using SimpleFacade.Exceptions;
using SimpleFacade.Validation;

namespace Demo.Domain.Tests.Utility
{
    public static class ValidationExtensions
    {
        public static void ShouldBeValid(this object executable)
        {
            Assert.DoesNotThrow(() => ExecutableValidator.Validate(executable));
        }

        public static void ShouldBeInvalid<TExecutable>(this TExecutable executable, Action<TExecutable> invalidate)
        {
            invalidate(executable);
            Assert.Throws<FacadeException>(() => ExecutableValidator.Validate(executable), executable + " passed validation, but expected failure");
        }
    }
}
