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
        public class FakeResponse
        {
            public int Value;

            protected FakeResponse()
            {
                Value = 123;
            }

            public FakeResponse(int value)
            {
                Value = value;
            }
        }

        public class FakeVoidCommand : ICommand { }

        public class FakeCommand : ICommand<FakeResponse> { }

        public class FakeQuerySingle : IQuerySingle<int> { }

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

        [Test]
        public void ExecuteVoidCommand_ReturnsNull()
        {
            var executor = new ExecutorStub();
            var executable = new FakeVoidCommand();

            var result = executor.Execute(executable);

            result.Should().BeNull();
        }

        [Test]
        public void ExecuteCommand_ReturnsDefaultConstructedResponse()
        {
            var executor = new ExecutorStub();

            var result = (FakeResponse)executor.Execute(new FakeCommand());

            result.ShouldBeEquivalentTo(new FakeResponse(123));
        }

        [Test]
        public void ExecuteQuerySingle_ReturnsDefaultValueType()
        {
            var executor = new ExecutorStub();

            var result = (int)executor.Execute(new FakeQuerySingle());

            result.Should().Be(0);
        }
    }
}
