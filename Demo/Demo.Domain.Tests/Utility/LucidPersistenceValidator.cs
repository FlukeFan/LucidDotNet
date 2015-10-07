using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using FluentAssertions;
using Lucid.Domain.Utility;

namespace Lucid.Domain.Tests.Utility
{
    public class LucidPersistenceValidator
    {
        private static readonly DateTime                    _minSqlServerDateTime   = new DateTime(1753, 1, 1, 0, 0, 0);
        private static readonly LucidPersistenceValidator   _validator              = new LucidPersistenceValidator();

        private static IDictionary<Type, Action<LucidPersistenceValidator, Entity>> _customValidators = new Dictionary<Type, Action<LucidPersistenceValidator, Entity>>();

        public static void BeforeSave(Entity entity)
        {
            var type = entity.GetType();
            var properties = type.GetProperties();

            foreach (var property in properties)
                CheckProperty(entity, property);

            if (_customValidators.ContainsKey(type))
                _customValidators[type](_validator, entity);
        }

        public static void AddCustomValidation<T>(Action<LucidPersistenceValidator, T> validation) where T : Entity
        {
            _customValidators.Add(typeof(T), (v, e) => validation(v, (T)e));
        }

        private static void CheckProperty(Entity entity, PropertyInfo property)
        {
            var type = property.PropertyType;

            if (type == typeof(DateTime))
                _validator.CheckMsSqlDateTime((DateTime)property.GetValue(entity), property.Name);
        }

        public void CheckMsSqlDateTime(DateTime dateTime, string propertyName)
        {
            dateTime.Should().BeOnOrAfter(_minSqlServerDateTime, propertyName + " value cannot be stored in SQL Server");
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
}
