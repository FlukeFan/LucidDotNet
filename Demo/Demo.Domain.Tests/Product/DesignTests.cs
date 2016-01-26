using System;
using Demo.Domain.Contract;
using Demo.Domain.Contract.Product.Commands;
using Demo.Domain.Product;
using Demo.Domain.Tests.Utility;
using FluentAssertions;
using Lucid.Domain.Testing;
using NUnit.Framework;

namespace Demo.Domain.Tests.Product
{
    public class DesignBuilder : Builder<Design>
    {
        static DesignBuilder()
        {
            DemoCustomInspections.Add<Design>((validator, entity) =>
            {
                validator.CheckNotNull(() => entity.Name);
                validator.CheckMaxLength(() => entity.Name, Constraints.DefaultMaxStringLength);
            });
        }

        public DesignBuilder()
        {
            With(u => u.Name, "test design 1");
        }
    }

    public class DesignTests : DomainTest
    {
        [Test]
        public void Construct()
        {
            new DesignBuilder().Save();
        }

        [Test]
        public void NameMustBeUnique()
        {
            new DesignBuilder().With(d => d.Name, "existingName").Save();

            Action act = () =>
            {
                Design.Start(new StartDesign { Name = "existingName" });
            };

            act.ShouldThrow<Exception>("duplicate names should not be allowed");
        }
    }
}
