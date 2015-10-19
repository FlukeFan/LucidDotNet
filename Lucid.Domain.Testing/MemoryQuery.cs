using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Lucid.Domain.Persistence;
using Lucid.Domain.Persistence.Queries;

namespace Lucid.Domain.Testing
{
    public class MemoryQuery
    {
        public static IList<T> Query<TId, T>(IList<IEntity<TId>> allEntities, Query<T, TId> query) where T : IEntity<TId>
        {
            var entities = allEntities.Where(e => typeof(T).IsAssignableFrom(e.GetType())).Cast<T>();

            foreach (var restriction in query.Restrictions)
                entities = Filter(entities, restriction);

            return entities.ToList();
        }

        private static IEnumerable<T> Filter<T>(IEnumerable<T> entities, Where restriction)
        {
            var where = (WhereBinaryComparison)restriction;
            var parameter = Expression.Parameter(typeof(T));
            var left = Expression.PropertyOrField(parameter, where.Operand1.Name);
            var right = Expression.Constant(where.Operand2);
            var comparison = Expression.MakeBinary(ExpressionType.Equal, left, right);
            var lambda = Expression.Lambda<Func<T, bool>>(comparison, parameter);
            var e = lambda.Compile();
            entities = entities.Where(e);
            return entities;
        }
    }
}