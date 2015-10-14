using System;
using FluentAssertions;
using Lucid.Domain.Testing;
using NUnit.Framework;

namespace Lucid.Domain.Tests.Testing
{
    [TestFixture]
    public class ConsistencyInspectorTests
    {
        public class FakeEntity
        {
            public object   Object      { get; set; }
            public DateTime DateTime    { get; set; }
            public string   String      { get; set; }
        }

        [Test]
        public void WhenMsSql_DateTimeIsValidated()
        {
            var inspector = new ConsistencyInspector();

            Assert.Throws<Exception>(() => inspector.BeforeSave(new FakeEntity { DateTime = DateTime.MinValue }));

            Assert.DoesNotThrow(() => inspector.BeforeSave(new FakeEntity { DateTime = new DateTime(2008, 07, 06) }));
            Assert.DoesNotThrow(() => inspector.BeforeSave(new FakeEntity { DateTime = DateTime.MaxValue }));
        }

        [Test]
        public void WhenNotMsSql_DateTimeIsIgnored()
        {
            var inspector = new ConsistencyInspector(isMsSql: false);

            Assert.DoesNotThrow(() => inspector.BeforeSave(new FakeEntity { DateTime = DateTime.MinValue }));
            Assert.DoesNotThrow(() => inspector.BeforeSave(new FakeEntity { DateTime = new DateTime(2008, 07, 06) }));
            Assert.DoesNotThrow(() => inspector.BeforeSave(new FakeEntity { DateTime = DateTime.MaxValue }));
        }

        [Test]
        public void Check_AllPropertiesValid()
        {
            var inspector = new ConsistencyInspector();
            var entity = new FakeEntity
            {
                Object = new object(),
                DateTime = new DateTime(2008, 07, 06),
                String = "not null",
            };

            Assert.DoesNotThrow(() => inspector.CheckNotNull(() => entity.Object));
            Assert.DoesNotThrow(() => inspector.CheckMsSqlDateTime(() => entity.DateTime));
            Assert.DoesNotThrow(() => inspector.CheckNotNullOrEmpty(() => entity.String));
        }

        [Test]
        public void CheckNotNull_ThrowsWhenNull()
        {
            var inspector = new ConsistencyInspector();
            var entity = new FakeEntity { Object = null };

            var e = Assert.Throws<Exception>(() => inspector.CheckNotNull(() => entity.Object));

            e.Message.Should().Contain("Object cannot be null");
        }

        [Test]
        public void CheckNotNullOrEmpty_Throws()
        {
            var inspector = new ConsistencyInspector();
            var entity = new FakeEntity { String = null };

            var e = Assert.Throws<Exception>(() => inspector.CheckNotNullOrEmpty(() => entity.String));

            e.Message.Should().Contain("String cannot be null");
        }

        [Test]
        public void CheckNotNullOrEmpty_ThrowsWhenEmpty()
        {
            var inspector = new ConsistencyInspector();
            var entity = new FakeEntity { String = "" };

            var e = Assert.Throws<Exception>(() => inspector.CheckNotNullOrEmpty(() => entity.String));

            e.Message.Should().Contain("String cannot be empty");
        }
    }
}
