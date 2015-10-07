using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Lucid.Domain.Testing
{
    public class Builder
    {
        /// <summary>
        ///  Utility method to create objects with protected constructor
        ///  e.g., Apple apple = ObjectBuilder.Create&lt;Apple&gt;();
        /// </summary>
        public static T Create<T>()
        {
            var constructors = typeof(T).GetConstructors(BindingFlags.NonPublic | BindingFlags.Instance);

            if ((constructors == null) || (constructors.Length == 0))
                throw new System.Exception("Couldn't find non-public default constructor on type " + typeof(T).Name);

            return (T)constructors[0].Invoke(null);
        }

        protected static PropertyInfo GetPropertyInfo(Expression body)
        {
            MemberExpression me;

            if (body is UnaryExpression)
                body = (body as UnaryExpression).Operand;

            me = (MemberExpression)body;
            return (PropertyInfo)me.Member;
        }

        public static string GetPropertyName(Expression body)
        {
            return GetPropertyInfo(body).Name;
        }
    }

    public class Builder<T> : Builder
    {
        protected T _instance;

        public Builder() : this(Create<T>()) { }

        public Builder(T instance)
        {
            if (instance == null)
                throw new ArgumentNullException("Cannot have null instance when constructing " + GetType());

            _instance = instance;
        }

        public Builder<T> With<U>(Expression<Func<T, U>> propertyFunction, U value)
        {
            PropertyInfo propertyInfo = GetPropertyInfo(propertyFunction.Body);

            if (propertyInfo.GetSetMethod() != null)
                throw new System.Exception("Property '" + propertyInfo.Name + "' is not protected on " + _instance.ToString());

            if (!propertyInfo.CanWrite)
                throw new System.Exception("Property '" + propertyInfo.Name + "' does not have a mutator on " + _instance.ToString());

            propertyInfo.SetValue(_instance, value, null);
            return this;
        }

        public T Value()
        {
            return _instance;
        }
    }
}
