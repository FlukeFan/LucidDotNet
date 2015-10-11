using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Lucid.Domain.Testing
{
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
            if (dateTime < MinSqlServerDateTime)
                throw new Exception(string.Format("DateTime property {0} with value {1} cannot be stored in SQL Server", propertyName, dateTime));
        }

        public void CheckNotNull(Expression<Func<string>> property)
        {
            CheckNotNull(property.Compile().Invoke(), Builder.GetPropertyName(property.Body));
        }

        public void CheckNotNull(string value, string propertyName)
        {
            if (value == null)
                throw new Exception(string.Format("string property {0} cannot be null", propertyName));
        }
    }
}
