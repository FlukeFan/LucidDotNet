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

        public WhereBinaryComparison(MemberInfo operand1, ExpressionType expressionType, object operand2)
        {
            Operand1 = operand1;
            Operator = FindOperator(expressionType);
            Operand2 = operand2;
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
