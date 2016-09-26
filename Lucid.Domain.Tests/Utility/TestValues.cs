using System;

namespace Lucid.Domain.Tests.Utility
{
    public class TestValues
    {
        public static DateTime EarlyDateTimeValue   = new DateTime(1800, 1, 1, 1, 2, 3);
        public static DateTime SummerDateTime1Value = new DateTime(2009, 08, 07, 06, 05, 04);

        public DateTime EarlyDateTime   = EarlyDateTimeValue;
        public DateTime SummerDateTime1 = SummerDateTime1Value;
    }
}
