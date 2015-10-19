﻿using FluentAssertions;
using Lucid.Domain.Persistence.Queries;
using NUnit.Framework;

namespace Lucid.Domain.Tests.Persistence.Queries
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
    }
}