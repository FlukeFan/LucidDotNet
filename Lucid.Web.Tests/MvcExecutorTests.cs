using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using FluentAssertions;
using NUnit.Framework;
using SimpleFacade;
using SimpleFacade.Exceptions;
using SimpleFacade.Execution;
using SimpleFacade.Testing;

namespace Lucid.Web.Tests
{
    [TestFixture]
    public class MvcExecutorTests
    {
        [Test]
        public void Exec_Query()
        {
            var stub = new ExecutorStub();
            var executor = new CqExecutor(stub);
            var expectedList = new List<int> { 1, 2, 3 };
            stub.SetupQuery(It.IsAny<Query>(), expectedList);

            var result = MvcExecutor.Exec(executor, new Query());

            result.Should().BeEquivalentTo(expectedList);
        }

        [Test]
        public void Exec_Command()
        {
            var executed = false;

            var state = new ModelStateDictionary();
            var stub = new ExecutorStub();
            var executor = new CqExecutor(stub);

            MvcExecutor.Exec(state, executor, new Cmd(),
                success: () => { executed = true; return null; },
                failure: () => { throw new Exception("should not fail"); });

            executed.Should().BeTrue();
        }

        [Test]
        public void Exec_Command_HandlesPropertyError()
        {
            var executed = false;

            var state = new ModelStateDictionary();
            var stub = new ExecutorStub();
            var executor = new CqExecutor(stub);

            var errors = new List<ValidationResult>
            {
                new ValidationResult("e1", new List<string> { "p1" }),
            };

            stub.SetupCommand<CmdOfT, int>((exe, def) => { throw new FacadeException(errors); });

            MvcExecutor.Exec(state, executor, new CmdOfT(),
                success: r => { throw new Exception("should not pass"); },
                failure: () => { executed = true; return null; });

            executed.Should().BeTrue();
            state.IsValid.Should().BeFalse();
            state["p1"].Errors[0].ErrorMessage.Should().Be("e1");
        }

        [Test]
        public void Exec_Command_HandlesNonPropertyError()
        {
            var executed = false;

            var state = new ModelStateDictionary();
            var stub = new ExecutorStub();
            var executor = new CqExecutor(stub);

            stub.SetupCommand<CmdOfT, int>((exe, def) => { throw new FacadeException("an error"); });

            MvcExecutor.Exec(state, executor, new CmdOfT(),
                success: r => { throw new Exception("should not pass"); },
                failure: () => { executed = true; return null; });

            executed.Should().BeTrue();
            state.IsValid.Should().BeFalse();
            state[""].Errors[0].ErrorMessage.Should().Be("an error");
        }

        public class Query : IQuery<IList<int>> { }
        public class Cmd : ICommand { }
        public class CmdOfT : ICommand<int> { }
    }
}
