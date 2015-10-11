using System;
using System.Reflection;
using Lucid.Domain.Testing;

namespace Demo.Domain.Tests.Utility
{
    public class DemoConsistencyInspector : ConsistencyInspector
    {
        protected override void CheckProperty(object entity, PropertyInfo property)
        {
            base.CheckProperty(entity, property);

            var type = property.PropertyType;

            if (type == typeof(DateTime))
                CheckMsSqlDateTime((DateTime)property.GetValue(entity), property.Name);
        }
    }
}
