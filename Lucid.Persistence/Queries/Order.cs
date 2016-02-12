using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Lucid.Persistence.Queries
{
    public enum Direction
    {
        Ascending,
        Descending,
    }

    public class Order
    {
        public Expression               KeyExpression           { get; private set; }
        public Type                     KeyType                 { get; private set; }
        public Direction                Direction               { get; private set; }

        public static Order For<T, TKey>(Expression<Func<T, TKey>> property, Direction direction)
        {
            return new Order
            {
                KeyExpression = property,
                KeyType = typeof(TKey),
                Direction = direction,
            };
        }

        public static Expression<Func<IEnumerable<T>, IOrderedEnumerable<T>>> Lambda<T>(Order order)
        {
            var parameter = Expression.Parameter(typeof(IEnumerable<T>));
            var extensionType = typeof(Enumerable);
            var method = order.Direction == Direction.Ascending ? "OrderBy" : "OrderByDescending";
            var typeArguments = new Type[] { typeof(T), order.KeyType };
            var arguments = new Expression[] { parameter, order.KeyExpression };
            var call = Expression.Call(extensionType, method, typeArguments, arguments);
            var lambda = Expression.Lambda<Func<IEnumerable<T>, IOrderedEnumerable<T>>>(call, parameter);
            return lambda;
        }
    }
}
