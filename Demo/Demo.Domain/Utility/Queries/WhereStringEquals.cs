using System;
using System.Linq.Expressions;

namespace Lucid.Domain.Utility.Queries
{
    public interface IWhereStringEqual : IWhere
    {
        string Value { get; }
    }

    public class WhereStringEqual<T> : Where<T>, IWhereStringEqual
    {
        private Func<T, string> _accessor;

        public string Value { get; private set; }

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
