using System;
using System.Collections.Generic;

namespace Lucid.Domain.Testing
{
    public static class CustomInspections
    {
        private static IDictionary<Type, Action<ConsistencyInspector, object>> _customValidators = new Dictionary<Type, Action<ConsistencyInspector, object>>();

        public static void Add<T>(Action<ConsistencyInspector, T> validation)
        {
            _customValidators.Add(typeof(T), (v, e) => validation(v, (T)e));
        }

        public static void Verify(Type type, object entity, ConsistencyInspector validator)
        {
            if (_customValidators.ContainsKey(type))
                _customValidators[type](validator, entity);
        }
    }
}
