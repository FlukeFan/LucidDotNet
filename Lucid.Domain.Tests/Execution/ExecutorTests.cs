﻿using System.Collections.Generic;
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
            var executor = new Executor() as IExecutor;

            var result = (int)executor.Execute(new SimpleExecutable());

            result.Should().Be(3);
        }

        [Test]
        public void ExecutesVoidCommandHandler()
        {
            VoidCommand.WasRun = false;
            var executor = new Executor().UsingHandlersFromAssemblyWithType<VoidCommandHandler>() as IExecutor;

            executor.Execute(new VoidCommand());

            VoidCommand.WasRun.Should().BeTrue();
        }

        [Test]
        public void ExecutesCommandHandler()
        {
            var executor = new Executor().UsingHandlersFromAssemblyWithType<VoidCommandHandler>() as IExecutor;

            var result = (int)executor.Execute(new Square { Value = 3 });

            result.Should().Be(9);
        }

        [Test]
        public void ExecutesQuerySingleHandler()
        {
            var executor = new Executor().UsingHandlersFromAssemblyWithType<VoidCommandHandler>() as IExecutor;

            var result = (int)executor.Execute(new Multiply { Value1 = 3, Value2 = 2 });

            result.Should().Be(6);
        }

        [Test]
        public void ExecutesQueryListHandler()
        {
            var executor = new Executor().UsingHandlersFromAssemblyWithType<VoidCommandHandler>() as IExecutor;

            var result = (IList<int>)executor.Execute(new List { Start = 4 });

            result.Should().ContainInOrder(4, 5, 6);
        }

        public class SimpleExecutable : QuerySingle<int>
        {
            public override int Find()
            {
                return 3;
            }
        }

        public class VoidCommand : ICommand
        {
            public static bool WasRun = false;
        }

        public class VoidCommandHandler : IHandleVoidCommand<VoidCommand>
        {
            public void Execute(VoidCommand cmd)
            {
                VoidCommand.WasRun = true;
            }
        }

        public class Square : ICommand<int>
        {
            public int Value;
        }

        public class SquareHandler : IHandleCommand<Square, int>
        {
            public int Execute(Square cmd)
            {
                return cmd.Value * cmd.Value;
            }
        }

        public class Multiply : IQuerySingle<int>
        {
            public int Value1;
            public int Value2;
        }

        public class MultipleHandle : IHandleQuerySingle<Multiply, int>
        {
            public int Find(Multiply query)
            {
                return query.Value1 * query.Value2;
            }
        }

        public class List : IQueryList<int>
        {
            public int Start;
        }

        public class ListHandler : IHandleQueryList<List, int>
        {
            public IList<int> List(List query)
            {
                return new List<int>
                {
                    query.Start,
                    query.Start + 1,
                    query.Start + 2,
                };
            }
        }
    }
}