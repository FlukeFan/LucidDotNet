using Lucid.Domain.Persistence;

namespace Demo.Domain.Utility
{
    public class DemoEntity : Entity<int>
    {
        public const int DefaultMaxStringLength = 255;
    }
}
