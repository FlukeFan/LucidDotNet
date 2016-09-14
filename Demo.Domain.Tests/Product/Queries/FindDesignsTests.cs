using System.Linq;
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
        public void List_OrdersByName()
        {
            new DesignBuilder().With(d => d.Name, "d2").Save();
            new DesignBuilder().With(d => d.Name, "d1").Save();
            new DesignBuilder().With(d => d.Name, "d3").Save();

            var designs = new FindDesignsHandler().Find(new FindDesigns());

            designs.Count.Should().Be(3);
            designs.Select(d => d.Name).Should().BeInAscendingOrder();
        }
    }
}
