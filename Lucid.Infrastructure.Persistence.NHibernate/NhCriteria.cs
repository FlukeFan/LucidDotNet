using System;
using System.Collections.Generic;
using Lucid.Domain.Persistence;
using Lucid.Domain.Persistence.Queries;
using NHibernate;
using NHibernate.Criterion;

namespace Lucid.Infrastructure.Persistence.NHibernate
{
    public static class NhCriteria
    {
        public static NhCriteria<T, TId> For<T, TId>(Query<T, TId> query) where T : IEntity<TId>
        {
            return new NhCriteria<T, TId>(query);
        }
    }

    public class NhCriteria<T, TId> where T : IEntity<TId>
    {
        private static IDictionary<Type, Action<ICriteria, Where>> _restrictionProcessors = new Dictionary<Type, Action<ICriteria, Where>>
        {
            { typeof(WhereBinaryComparison), (criteria, where) => AddBinaryComparison(criteria, (WhereBinaryComparison)where) },
        };

        private static IDictionary<WhereBinaryComparison.OperatorType, Func<string, object, ICriterion>> _binaryComparisons = new Dictionary<WhereBinaryComparison.OperatorType, Func<string, object, ICriterion>>
        {
            { WhereBinaryComparison.OperatorType.LessThan,              (prop, val) => Restrictions.Lt(prop, val) },
            { WhereBinaryComparison.OperatorType.LessThanOrEqual,       (prop, val) => Restrictions.Le(prop, val) },
            { WhereBinaryComparison.OperatorType.Equal,                 (prop, val) => Restrictions.Eq(prop, val) },
            { WhereBinaryComparison.OperatorType.GreaterThanOrEqual,    (prop, val) => Restrictions.Ge(prop, val) },
            { WhereBinaryComparison.OperatorType.GreaterThan,           (prop, val) => Restrictions.Gt(prop, val) },
        };

        public Query<T, TId> Query { get; protected set; }

        public NhCriteria(Query<T, TId> query)
        {
            Query = query;
        }

        public ICriteria CreateCriteria(ISession session)
        {
            var criteria = session.CreateCriteria(typeof(T));

            foreach (var restriction in Query.Restrictions)
                AddRestriction(criteria, restriction);

            return criteria;
        }

        private void AddRestriction(ICriteria criteria, Where where)
        {
            var whereType = where.GetType();

            if (!_restrictionProcessors.ContainsKey(whereType))
                throw new Exception("Unhandled Where clause: " + where);

            var processor = _restrictionProcessors[whereType];
            processor(criteria, where);
        }

        private static void AddBinaryComparison(ICriteria criteria, WhereBinaryComparison where)
        {
            if (!_binaryComparisons.ContainsKey(where.Operator))
                throw new Exception("Unhandled comparison operator: " + where.Operator);

            var criterionFunc = _binaryComparisons[where.Operator];
            var criterion = criterionFunc(where.Operand1.Name, where.Operand2);
            criteria.Add(criterion);
        }
    }
}
