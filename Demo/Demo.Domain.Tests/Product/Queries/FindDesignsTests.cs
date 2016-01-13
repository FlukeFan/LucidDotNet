using Demo.Domain.Contract.Product.Queries;
using Demo.Domain.Product.Queries;
using Demo.Domain.Tests.Utility;
using FluentAssertions;
using NUnit.Framework;

namespace Demo.Domain.Tests.Product.Queries
{
    [TestFixture]
    public class FindDesignsTests : DomainTest
    {
        [Test]
        public void Execute()
        {
            new DesignBuilder().With(d => d.Name, "d2").Save();
            new DesignBuilder().With(d => d.Name, "d1").Save();
            new DesignBuilder().With(d => d.Name, "d3").Save();

            var designs = new FindDesignsHandler().List(new FindDesigns());

            designs.Count.Should().Be(3);
            designs[0].Name.Should().Be("d2");
            designs[1].Name.Should().Be("d1");
            designs[2].Name.Should().Be("d3");
        }
    }
}
