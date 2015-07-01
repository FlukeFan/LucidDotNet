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

        public abstract bool Satisfies(T entity);

        public static Where<T> PropEq(Expression<Func<T, string>> property, string value)
        {
            return new WhereStringEqual<T>(property, value);
        }
    }

    public class WhereStringEqual<T> : Where<T>
    {
        private Func<T, string> _accessor;

        public string  Value { get; private set; }

        public WhereStringEqual(Expression<Func<T, string>> property, string value)
            : base(property.Body)
        {
            _accessor = property.Compile();
            Value = value;
        }

        public override bool Satisfies(T entity)
        {
            var entityValue = _accessor(entity);
            return entityValue == Value;
        }
    }
}
