﻿using System;
using FluentAssertions;
using Lucid.Persistence.Queries;
using NUnit.Framework;

namespace Lucid.Persistence.Tests.Queries
{
    [TestFixture]
    public class WhereTests
    {
        [Test]
        public void SimpleComparison()
        {
            var where = Where.For<LucidPolyType>(p => p.String == "test string")
                .As<WhereBinaryComparison>();

            where.Operand1.Name.Should().Be("String");
            where.Operator.Should().Be(WhereBinaryComparison.OperatorType.Equal);
            where.Operand2.Should().Be("test string");
        }

        [Test]
        public void NotAllowedExpression()
        {
            var exception = Assert.Throws<Exception>(() => Where.For<LucidPolyType>(p => char.IsNumber(p.String[0])));

            exception.Message.Should().StartWith("Unable to form query for:");
        }
    }
}