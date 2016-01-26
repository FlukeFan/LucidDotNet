using System;
using System.ComponentModel.DataAnnotations;
using FluentAssertions;
using Lucid.Domain.Exceptions;
using Lucid.Domain.Execution;
using Lucid.Domain.Validation;
using NUnit.Framework;

namespace Lucid.Domain.Tests.Validation
{
    [TestFixture]
    public class ValildatingExecutorTests
    {
        [Test]
        public void Execute_ValidatesDto()
        {
            Action act = () =>
            {
                var executor = (IExecutor)new ValidatingExecutor(new Executor());
                executor.Execute(new Dto { Name = null });
            };

            act.ShouldThrow<LucidException>();
        }

        public class Dto
        {
            [Required]
            public string Name { get; set; }
        }
    }
}
