using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Lucid.Domain.Persistence.Queries
{
    public interface IWhere
    {
        PropertyInfo Property { get; }
    }

    public abstract class Where<T> : IWhere
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
}
