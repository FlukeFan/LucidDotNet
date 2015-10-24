using Demo.Domain.Product;
using Demo.Domain.Tests.Product;
using Demo.Infrastructure.Tests.Utility;
using FluentAssertions;
using NUnit.Framework;

namespace Demo.Infrastructure.Tests.Product
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
        }
    }
}
