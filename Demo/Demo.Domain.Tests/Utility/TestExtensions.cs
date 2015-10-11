using System;
using Lucid.Domain.Remote;
using NUnit.Framework;

namespace Demo.Domain.Tests.Utility
{
    public static class TestExtensions
    {
        public static void ShouldBeValid(this IRemoteable remoteable)
        {
            Validator.Validate(remoteable);
        }

        public static void ShouldBeInvalid<T>(this T remoteable, Action<T> invalidate) where T : IRemoteable
        {
            invalidate(remoteable);
            Assert.Throws<Exception>(() => Validator.Validate(remoteable), remoteable + " passed validation, but expected failure");
        }
    }
}
