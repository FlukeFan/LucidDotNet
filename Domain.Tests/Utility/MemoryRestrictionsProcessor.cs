using System;
using System.Collections.Generic;
using Lucid.Domain.Utility.Queries;
using FluentAssertions;

namespace Lucid.Domain.Tests.Utility
{
    public class MemoryRestrictionsProcessor
    {
        private static IDictionary<Type, object> _cache = new Dictionary<Type, object>();

        public static MemoryRestrictionsProcessor<T> For<T>()
        {
            if (!_cache.ContainsKey(typeof(T)))
                _cache.Add(typeof(T), new MemoryRestrictionsProcessor<T>());

            var restrictions = _cache[typeof(T)];
            return (MemoryRestrictionsProcessor<T>)restrictions;
        }
    }

    public class MemoryRestrictionsProcessor<T>
    {
        private IDictionary<Type, Func<T, Where<T>, bool>> _processors = new Dictionary<Type, Func<T, Where<T>, bool>>();

        public MemoryRestrictionsProcessor()
        {
            _processors.Add(typeof(WhereStringEqual<T>), WhereStringEqual);
        }

        public bool Satisfies(T entity, Where<T> where)
        {
            _processors.ContainsKey(where.GetType()).Should().BeTrue(where.GetType() + " not implemented");
            var processor = _processors[where.GetType()];
            return processor(entity, where);
        }

        private bool WhereStringEqual(T entity, Where<T> where)
        {
            var whereStringEqual = (WhereStringEqual<T>)where;
            var propertyValue = (string)where.Property.GetValue(entity);
            return propertyValue == whereStringEqual.Value;
        }
    }
}
