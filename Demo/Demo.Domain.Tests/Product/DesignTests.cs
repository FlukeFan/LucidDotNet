using Demo.Domain.Product;
using Demo.Domain.Tests.Utility;
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
            });
        }

        public DesignBuilder()
        {
            With(u => u.Name, "test design 1");
        }
    }

    [TestFixture]
    public class DesignTests : DomainTest
    {
        [Test]
        public void Construct()
        {
            new DesignBuilder().Save();
        }
    }
}
