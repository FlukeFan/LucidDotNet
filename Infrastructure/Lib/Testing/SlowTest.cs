using NUnit.Framework;

namespace Lucid.Infrastructure.Lib.Testing
{
    public class SlowTest : CategoryAttribute
    {
        public SlowTest() : base("Slow") { }
    }
}
