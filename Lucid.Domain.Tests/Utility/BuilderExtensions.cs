using Lucid.Domain.Utility;
using Reposify.Testing;

namespace Lucid.Domain.Tests.Utility
{
    public static class BuilderExtensions
    {
        public static T Save<T>(this Builder<T> builder) where T : LucidEntity
        {
            return DomainRegistry.Repository.Save(builder.Value());
        }
    }
}
