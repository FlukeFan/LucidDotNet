using System;
using Lucid.Domain.Contract;
using Lucid.Domain.Contract.Product.Commands;
using Lucid.Domain.Product;
using Lucid.Domain.Tests.Utility;
using Lucid.Domain.Utility;
using FluentAssertions;
using NUnit.Framework;
using Reposify.Testing;

namespace Lucid.Domain.Tests.Product
{
    public class DesignBuilder : Builder<Design>
    {
        static DesignBuilder()
        {
            LucidCustomInspections.Add<Design>((validator, entity) =>
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

            act.ShouldThrow<LucidException>("duplicate names should not be allowed");
        }
    }
}
