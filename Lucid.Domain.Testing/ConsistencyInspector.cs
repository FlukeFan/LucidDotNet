using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Lucid.Domain.Testing
{
    public class ConsistencyInspector
    {
        public static readonly DateTime MinSqlServerDateTime = new DateTime(1753, 1, 1, 0, 0, 0);

        private bool _isMsSql;

        public ConsistencyInspector(bool isMsSql = true)
        {
            _isMsSql = isMsSql;
        }

        public virtual void BeforeSave(object entity)
        {
            var type = entity.GetType();
            var properties = type.GetProperties();

            foreach (var property in properties)
                CheckProperty(entity, property);
        }

        protected virtual void CheckProperty(object entity, PropertyInfo property)
        {
            var type = property.PropertyType;

            if (type == typeof(DateTime) && _isMsSql)
                CheckMsSqlDateTime((DateTime)property.GetValue(entity), property.Name);
        }

        public void CheckMsSqlDateTime(Expression<Func<DateTime>> property)
        {
            CheckMsSqlDateTime(property.Compile().Invoke(), Builder.GetPropertyName(property.Body));
        }

        public void CheckMsSqlDateTime(DateTime dateTime, string propertyName)
        {
            if (dateTime < MinSqlServerDateTime)
                throw new Exception(string.Format("DateTime property {0} with value {1} cannot be stored in SQL Server", propertyName, dateTime));
        }

        public void CheckNotNull(Expression<Func<object>> property)
        {
            CheckNotNull(property.Compile().Invoke(), Builder.GetPropertyName(property.Body));
        }

        public void CheckNotNull(object value, string propertyName)
        {
            if (value == null)
                throw new Exception(string.Format("property {0} cannot be null", propertyName));
        }

        public void CheckNotNullOrEmpty(Expression<Func<string>> property)
        {
            CheckNotNullOrEmpty(property.Compile().Invoke(), Builder.GetPropertyName(property.Body));
        }

        public void CheckNotNullOrEmpty(string value, string propertyName)
        {
            CheckNotNull(value, propertyName);

            if (value == string.Empty)
                throw new Exception(string.Format("string property {0} cannot be empty", propertyName));
        }
    }
}
