using FluentAssertions;
using Lucid.Domain.Execution;
using NUnit.Framework;

namespace Lucid.Domain.Tests.Execution
{
    [TestFixture]
    public class ExecutorTests
    {
        [Test]
        public void Executes()
        {
            var executor = new Executor();

            var result = (int)executor.Execute(new SimpleExecutable());

            result.Should().Be(3);
        }

        public class SimpleExecutable : QuerySingle<int>
        {
            public override int Find()
            {
                return 3;
            }
        }
    }
}
