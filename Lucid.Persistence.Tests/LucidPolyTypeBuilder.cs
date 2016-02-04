using System;
using Lucid.Persistence.Testing;

namespace Lucid.Persistence.Tests
{
    public class LucidPolyTypeBuilder : Builder<LucidPolyType>
    {
        public static readonly DateTime DefaultDateTimeValue = new DateTime(2001, 02, 03, 04, 05, 06);

        public LucidPolyTypeBuilder()
        {
            With(u => u.String, "string value");
            With(u => u.Int, 10);
            With(u => u.DateTime, DefaultDateTimeValue);
            With(u => u.Enum, LucidPolyType.Values.Val2);
        }
    }
}
