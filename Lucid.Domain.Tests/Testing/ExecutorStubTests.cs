using System.Linq;
using FluentAssertions;
using Lucid.Domain.Execution;
using Lucid.Domain.Testing;
using NUnit.Framework;

namespace Lucid.Domain.Tests.Testing
{
    [TestFixture]
    public class ExecutorStubTests
    {
        public class FakeVoidCommand : ICommand { }

        [Test]
        public void StoresAllExecutions()
        {
            var executor = new ExecutorStub();
            var executable = new FakeVoidCommand();

            executor.Execute(executable);

            executor.Executed.Count().Should().Be(1);
            executor.Executed.ElementAt(0).Should().Be(executable);
        }
    }
}
