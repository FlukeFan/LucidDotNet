﻿using System;
using Lucid.Domain.Execution;
using NUnit.Framework;

namespace Demo.Domain.Tests.Utility
{
    public static class ValidationExtensions
    {
        public static void ShouldBeValid(this object executable)
        {
            Validator.Validate(executable);
        }

        public static void ShouldBeInvalid<TExecutable>(this TExecutable executable, Action<TExecutable> invalidate)
        {
            invalidate(executable);
            Assert.Throws<Exception>(() => Validator.Validate(executable), executable + " passed validation, but expected failure");
        }
    }
}