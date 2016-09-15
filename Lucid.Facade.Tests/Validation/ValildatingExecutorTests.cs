using System;
using System.ComponentModel.DataAnnotations;
using FluentAssertions;
using Lucid.Facade.Exceptions;
using Lucid.Facade.Execution;
using Lucid.Facade.Validation;
using NUnit.Framework;

namespace Lucid.Facade.Tests.Validation
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

            act.ShouldThrow<FacadeException>();
        }

        public class Dto
        {
            [Required]
            public string Name { get; set; }
        }
    }
}
