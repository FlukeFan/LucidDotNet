using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Lucid.Domain.Persistence.Queries
{
    public class WhereBinaryComparison : Where
    {
        public enum OperatorType
        {
            Equal,
        }

        public MemberInfo   Operand1 { get; protected set; }
        public OperatorType Operator { get; protected set; }
        public object       Operand2 { get; protected set; }

        public WhereBinaryComparison(Expression expression, MemberInfo operand1, ExpressionType expressionType, object operand2) : base(expression)
        {
            Operand1 = operand1;
            Operator = FindOperator(expressionType);
            Operand2 = operand2;
        }

        public override Expression CreateExpression(ParameterExpression parameter)
        {
            var left = Expression.PropertyOrField(parameter, Operand1.Name);
            var right = Expression.Constant(Operand2);
            var comparison = Expression.MakeBinary(ExpressionType.Equal, left, right);
            return comparison;
        }

        private OperatorType FindOperator(ExpressionType expressionType)
        {
            switch(expressionType)
            {
                case ExpressionType.Equal:
                    return OperatorType.Equal;
                default:
                    throw new Exception("Unhandled binary comparison operation: " + expressionType);
            }
        }
    }
}
