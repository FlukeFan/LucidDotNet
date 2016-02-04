using System;
using Lucid.Facade.Exceptions;
using Lucid.Facade.Validation;
using NUnit.Framework;

namespace Demo.Domain.Tests.Utility
{
    public static class ValidationExtensions
    {
        public static void ShouldBeValid(this object executable)
        {
            LucidValidator.Validate(executable);
        }

        public static void ShouldBeInvalid<TExecutable>(this TExecutable executable, Action<TExecutable> invalidate)
        {
            invalidate(executable);
            Assert.Throws<LucidException>(() => LucidValidator.Validate(executable), executable + " passed validation, but expected failure");
        }
    }
}
