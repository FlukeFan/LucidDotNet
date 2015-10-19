using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Lucid.Domain.Persistence.Queries
{
    public abstract class Where
    {
        private enum ExpressionTypes
        {
            Unrecognised,
            BinaryExpression,
            MemberExpression,
        };

        public static Where For<T>(Expression<Func<T, bool>> restriction)
        {
            return For(restriction.Body);
        }

        private static Where For(Expression restriction)
        {
            switch(FindType(restriction))
            {
                case ExpressionTypes.BinaryExpression:
                    return ForBinaryExpression((BinaryExpression)restriction);
                default:
                    throw NewException("Unable to form query for: ", restriction);
            }
        }

        private static Where ForBinaryExpression(BinaryExpression binaryExpression)
        {
            var operand1 = FindMemberInfo(binaryExpression.Left);
            var operand2 = FindValue(binaryExpression.Right);
            return new WhereBinaryComparison(operand1, binaryExpression.NodeType, operand2);
        }

        private static MemberInfo FindMemberInfo(Expression expression)
        {
            switch(FindType(expression))
            {
                case ExpressionTypes.MemberExpression:
                    return FindMemberInfo((MemberExpression)expression);
                default:
                    throw NewException("Expected property access (like e.Name).  Unabled to find MemberInfo for: ", expression);
            }
        }

        private static MemberInfo FindMemberInfo(MemberExpression memberExpression)
        {
            return memberExpression.Member;
        }

        private static object FindValue(Expression expression)
        {
            var valueExpression = Expression.Lambda(expression).Compile();
            object value = valueExpression.DynamicInvoke();
            return value;
        }

        private static ExpressionTypes FindType(Expression expression)
        {
            if (expression is BinaryExpression)
                return ExpressionTypes.BinaryExpression;
            else if (expression is MemberExpression)
                return ExpressionTypes.MemberExpression;
            else
                return ExpressionTypes.Unrecognised;
        }

        private static Exception NewException(string message, Expression expression)
        {
            return new Exception(string.Format("{0} {1} of type ({2}, {3})", message, expression, expression.NodeType, expression.GetType()));
        }
    }
}
