using Demo.Domain.Tests.Product;
using Demo.Infrastructure.Tests.Utility;
using NUnit.Framework;

namespace Demo.Infrastructure.Tests.Product
{
    public class ProductMappingTests : DbTest
    {
        [Test]
        public void Design()
        {
            var design = new DesignBuilder().Value();

            Repository.Save(design);
        }
    }
}
