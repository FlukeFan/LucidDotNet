using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using FluentAssertions;
using Lucid.Domain.Testing;

namespace Lucid.Domain.Tests.Utility
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

    public class ConsistencyInspector
    {
        public static readonly DateTime MinSqlServerDateTime = new DateTime(1753, 1, 1, 0, 0, 0);

        public virtual void BeforeSave(object entity)
        {
            var type = entity.GetType();
            var properties = type.GetProperties();

            foreach (var property in properties)
                CheckProperty(entity, property);

            CustomInspections.Verify(type, entity, this);
        }

        protected virtual void CheckProperty(object entity, PropertyInfo property)
        {
        }

        public void CheckMsSqlDateTime(DateTime dateTime, string propertyName)
        {
            dateTime.Should().BeOnOrAfter(MinSqlServerDateTime, propertyName + " value cannot be stored in SQL Server");
        }

        public void CheckNotNull(Expression<Func<string>> property)
        {
            CheckNotNull(property.Compile().Invoke(), Builder.GetPropertyName(property.Body));
        }

        public void CheckNotNull(string value, string propertyName)
        {
            value.Should().NotBeNullOrWhiteSpace(propertyName + " value cannot be null");
        }
    }

    public static class DemoCustomInspections
    {
        public static void Add<T>(Action<DemoConsistencyInspector, T> inspection)
        {
            CustomInspections.Add<T>((ci, entity) => inspection((DemoConsistencyInspector)ci, entity));
        }
    }

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
