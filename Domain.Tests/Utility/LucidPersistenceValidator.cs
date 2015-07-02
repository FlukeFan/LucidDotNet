using System;
using System.Reflection;
using FluentAssertions;
using Lucid.Domain.Utility;

namespace Lucid.Domain.Tests.Utility
{
    public static class LucidPersistenceValidator
    {
        private static readonly DateTime _minSqlServerDateTime = new DateTime(1753, 1, 1, 0, 0, 0);

        public static void BeforeSave(Entity entity)
        {
            var properties = entity.GetType().GetProperties();

            foreach (var property in properties)
                CheckProperty(entity, property);
        }

        private static void CheckProperty(Entity entity, PropertyInfo property)
        {
            var type = property.PropertyType;

            if (type == typeof(DateTime))
                CheckMsSqlDateTime((DateTime)property.GetValue(entity), property.Name);
        }

        public static void CheckMsSqlDateTime(DateTime dateTime, string propertyName)
        {
            dateTime.Should().BeOnOrAfter(_minSqlServerDateTime, propertyName + " value cannot be stored in SQL Server");
        }
    }
}
