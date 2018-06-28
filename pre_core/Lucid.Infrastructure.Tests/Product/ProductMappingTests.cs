using Lucid.Domain.Contract;
using Lucid.Domain.Product;
using Lucid.Domain.Tests.Product;
using Lucid.Infrastructure.Tests.Utility;
using FluentAssertions;
using NUnit.Framework;

namespace Lucid.Infrastructure.Tests.Product
{
    public class ProductMappingTests : DbTest
    {
        [Test]
        public void Design_SaveLoad()
        {
            var design = new DesignBuilder()
                .With(d => d.Name, "test name")
                .Value();

            Repository.Save(design);

            Repository.Flush();
            Repository.Clear();

            var loadedDesign = Repository.Load<Design>(design.Id);

            loadedDesign.Id.Should().Be(design.Id);
            loadedDesign.Name.Should().Be(design.Name);
        }

        [Test]
        public void Design_VerifyConstraints()
        {
            VerifyInvalidConstraint(new DesignBuilder().With(d => d.Name, null).Value());
            VerifyInvalidConstraint(new DesignBuilder().With(d => d.Name, new string('x', Constraints.DefaultMaxStringLength + 1)).Value());
        }
    }
}
