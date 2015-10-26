using System;
using System.Linq.Expressions;
using Lucid.Domain.Testing;

namespace Demo.Domain.Tests.Utility
{
    public class DemoConsistencyInspector : ConsistencyInspector
    {
        public DemoConsistencyInspector() : base(isMsSql: true) { }

        public void CheckMaxLength(Expression<Func<string>> property, int maxLength)
        {
            CheckMaxLength(property.Compile().Invoke(), maxLength, Builder.GetPropertyName(property.Body));
        }

        public void CheckMaxLength(string value, int maxLength, string propertyName)
        {
            if (value != null && value.Length > maxLength)
                throw new Exception(string.Format("property {0} has size {1} which is larger than maximum allowable of {2}", propertyName, value.Length, maxLength));
        }
    }
}
