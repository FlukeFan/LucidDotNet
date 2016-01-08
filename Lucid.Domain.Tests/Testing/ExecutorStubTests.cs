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
        public void AllExecuted_ReturnsStoredExecutions()
        {
            var executor = new ExecutorStub();
            var executable = new FakeVoidCommand();

            executor.Execute(executable);

            executor.AllExecuted().Count().Should().Be(1);
            executor.AllExecuted().ElementAt(0).Should().Be(executable);
        }

        [Test]
        public void ExecutedOfType_ReturnsExecutionsOfType()
        {
            var executor = new ExecutorStub();
            var executable = new FakeVoidCommand();

            executor.Execute(executable);

            executor.Executed<FakeVoidCommand>().Count().Should().Be(1);
            executor.Executed<FakeVoidCommand>().ElementAt(0).Should().Be(executable);

            executor.Executed<ExecutorStubTests>().Count().Should().Be(0);
        }
    }
}
