using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Lucid.Domain.Utility.Queries
{
    public abstract class Where<T>
    {
        public PropertyInfo Property { get; private set; }

        public Where(Expression body)
        {
            MemberExpression me;

            if (body is UnaryExpression)
                body = (body as UnaryExpression).Operand;

            me = (MemberExpression)body;
            Property = (PropertyInfo)me.Member;
        }

        public static Where<T> Equals(Expression<Func<T, string>> property, string value)
        {
            return new WhereStringEqual<T>(property.Body, value);
        }
    }

    public class WhereStringEqual<T> : Where<T>
    {
        public string  Value { get; private set; }

        public WhereStringEqual(Expression property, string value) : base(property)
        {
            Value = value;
        }
    }
}
