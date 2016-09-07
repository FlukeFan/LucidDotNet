using FluentAssertions.Primitives;

namespace Demo.Web.Tests.Utility
{
    public static class ShouldExtensions
    {
        public static void BeAction(this StringAssertions stringAssertion, string action)
        {
            stringAssertion.StartWith(action.Replace("~", ""));
        }
    }
}
