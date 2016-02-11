using System.Collections.Generic;
using FluentAssertions;
using Lucid.Facade.Execution;
using Lucid.Facade.Testing;
using NUnit.Framework;

namespace Lucid.Web.Tests
{
    [TestFixture]
    public class MvcExecutorTests
    {
        [Test]
        public void Exec_QueryList()
        {
            var stub = new ExecutorStub();
            var executor = new CqExecutor(stub);
            var expectedList = new List<int> { 1, 2, 3 };
            stub.SetupQueryList(It.IsAny<QueryList>(), expectedList);

            var result = MvcExecutor.Exec(executor, new QueryList());

            result.Should().BeEquivalentTo(expectedList);
        }

        [Test]
        public void Exec_QuerySingle()
        {
            var stub = new ExecutorStub();
            var executor = new CqExecutor(stub);
            stub.SetupQuerySingle(It.IsAny<QuerySingle>(), 5);

            var result = MvcExecutor.Exec(executor, new QuerySingle());

            result.Should().Be(5);
        }

        public class QueryList : IQueryList<int> { }
        public class QuerySingle : IQuerySingle<int> { }
    }
}
