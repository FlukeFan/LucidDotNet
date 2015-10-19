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
            var where = Where.For<LucidPolyType>(p => p.Email == "test email")
                .As<WhereBinaryComparison>();

            where.Operand1.Name.Should().Be("Email");
            where.Operator.Should().Be(WhereBinaryComparison.OperatorType.Equal);
            where.Operand2.Should().Be("test email");
        }
    }
}
