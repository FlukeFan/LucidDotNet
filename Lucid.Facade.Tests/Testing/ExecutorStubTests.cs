﻿using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Lucid.Facade.Execution;
using Lucid.Facade.Testing;
using NUnit.Framework;

namespace Lucid.Facade.Tests.Testing
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

        public class FakeCommand : ICommand<FakeResponse> { public int Input = 5; }

        public class FakeQueryValue : IQuery<int> { }

        public class FakeQueryList : IQuery<IList<FakeResponse>> { };

        public class FakeQueryEnumerable : IQuery<IEnumerable<FakeResponse>> { }

        public class FakeQueryDictionary : IQuery<IDictionary<int, FakeResponse>> { }

        public class FakeQueryArray : IQuery<int[]> { }

        [Test]
        public void Executed_ReturnsStoredExecutions()
        {
            var executor = new ExecutorStub();
            var executable = new FakeVoidCommand();

            (executor as IExecutor).Execute(executable);

            executor.Executed().Count().Should().Be(1);
            executor.Executed().ElementAt(0).Should().Be(executable);
        }

        [Test]
        public void ExecutedOfType_ReturnsExecutionsOfType()
        {
            var executor = new ExecutorStub();
            var executable = new FakeVoidCommand();

            (executor as IExecutor).Execute(executable);

            executor.Executed<FakeVoidCommand>().Count().Should().Be(1);
            executor.Executed<FakeVoidCommand>().ElementAt(0).Should().Be(executable);

            executor.Executed<ExecutorStubTests>().Count().Should().Be(0);
        }

        [Test]
        public void ExecuteVoidCommand_ReturnsNull()
        {
            var executor = new ExecutorStub();
            var executable = new FakeVoidCommand();

            var result = (executor as IExecutor).Execute(executable);

            result.Should().BeNull();
        }

        [Test]
        public void ExecuteCommand_ReturnsDefaultConstructedResponse()
        {
            var executor = new ExecutorStub();

            var result = (FakeResponse)(executor as IExecutor).Execute(new FakeCommand());

            result.ShouldBeEquivalentTo(new FakeResponse(123));
        }

        [Test]
        public void ExecuteQuerySingle_ReturnsDefaultValueType()
        {
            var executor = new ExecutorStub();

            var result = (int)(executor as IExecutor).Execute(new FakeQueryValue());

            result.Should().Be(0);
        }

        [Test]
        public void ExecuteQueryList_ReturnsEmptyList()
        {
            var executor = new ExecutorStub();

            var result = (IList<FakeResponse>)(executor as IExecutor).Execute(new FakeQueryList());

            result.Should().NotBeNull();
            result.Count.Should().Be(0);
        }

        [Test]
        public void ExecuteQueryEnumerable_ReturnsEmptyList()
        {
            var executor = new ExecutorStub();

            var result = (IEnumerable<FakeResponse>)(executor as IExecutor).Execute(new FakeQueryEnumerable());

            result.Should().NotBeNull();
            result.Count().Should().Be(0);
        }

        [Test]
        public void ExecuteQueryDictionary_ReturnsEmptyDictionary()
        {
            var executor = new ExecutorStub();

            var result = (IDictionary<int, FakeResponse>)(executor as IExecutor).Execute(new FakeQueryDictionary());

            result.Should().NotBeNull();
            result.Keys.Count().Should().Be(0);
        }

        [Test]
        public void ExecuteQueryArray_ReturnsEmptyArray()
        {
            var executor = new ExecutorStub();

            var result = (int[])(executor as IExecutor).Execute(new FakeQueryArray());

            result.Should().NotBeNull();
            result.Length.Should().Be(0);
        }

        [Test]
        public void SetupObjectResult()
        {
            var executor = new ExecutorStub()
                .SetupObjectResult<FakeCommand>(new FakeResponse(234));

            var result = (FakeResponse)(executor as IExecutor).Execute(new FakeCommand());

            result.ShouldBeEquivalentTo(new FakeResponse(234));
        }

        [Test]
        public void SetupObjectLambdaResult()
        {
            var executor = new ExecutorStub()
                .SetupObjectResult<FakeCommand>((exe, def) =>
                {
                    var e = (FakeCommand)exe;
                    if (e.Input == 10)
                        return new FakeResponse(10);
                    else
                        return def;
                });

            var result1 = (FakeResponse)(executor as IExecutor).Execute(new FakeCommand { Input = 10 });
            var result2 = (FakeResponse)(executor as IExecutor).Execute(new FakeCommand { Input = 20 });

            result1.ShouldBeEquivalentTo(new FakeResponse(10));
            result2.ShouldBeEquivalentTo(new FakeResponse(123));
        }

        [Test]
        public void SetupCommandResult()
        {
            var executor = new ExecutorStub()
                .SetupCommand(It.IsAny<FakeCommand>(), new FakeResponse(456));

            var result = (FakeResponse)(executor as IExecutor).Execute(new FakeCommand());

            result.ShouldBeEquivalentTo(new FakeResponse(456));
        }

        [Test]
        public void SetupVoidCommand()
        {
            var state = 0;

            var executor = new ExecutorStub()
                .SetupVoidCommand(It.IsAny<FakeVoidCommand>(), (cmd) => { state = 5; });

            state.Should().Be(0);

            (executor as IExecutor).Execute(new FakeVoidCommand());

            state.Should().Be(5);
        }

        [Test]
        public void SetupQueryListResult()
        {
            var executor = new ExecutorStub()
                .SetupQuery(It.IsAny<FakeQueryList>(),
                new List<FakeResponse>
                {
                    new FakeResponse(567),
                    new FakeResponse(678),
                });

            var result = (IList<FakeResponse>)(executor as IExecutor).Execute(new FakeQueryList());

            result.Should().NotBeNull();
            result.Count.Should().Be(2);
            result[0].Value.Should().Be(567);
            result[1].Value.Should().Be(678);
        }

        [Test]
        public void SetupQuerySingleResult()
        {
            var executor = new ExecutorStub()
                .SetupQuery(It.IsAny<FakeQueryValue>(), 7);

            var result = (int)(executor as IExecutor).Execute(new FakeQueryValue());

            result.Should().Be(7);
        }
    }
}
