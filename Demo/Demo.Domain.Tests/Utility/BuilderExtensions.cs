using Demo.Domain.Utility;
using Lucid.Persistence.Testing;

namespace Demo.Domain.Tests.Utility
{
    public static class BuilderExtensions
    {
        public static T Save<T>(this Builder<T> builder) where T : DemoEntity
        {
            return Registry.Repository.Save(builder.Value());
        }
    }
}
