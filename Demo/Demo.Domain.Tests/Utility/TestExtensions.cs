using System;
using Lucid.Domain.Execution;
using NUnit.Framework;

namespace Demo.Domain.Tests.Utility
{
    public static class TestExtensions
    {
        public static void ShouldBeValid(this IExecutable executable)
        {
            Validator.Validate(executable);
        }

        public static void ShouldBeInvalid<Executable>(this Executable executable, Action<Executable> invalidate) where Executable : IExecutable
        {
            invalidate(executable);
            Assert.Throws<Exception>(() => Validator.Validate(executable), executable + " passed validation, but expected failure");
        }
    }
}
