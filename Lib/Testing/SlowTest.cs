using NUnit.Framework;

namespace Lucid.Lib.Testing
{
    public class SlowTest : CategoryAttribute
    {
        public SlowTest() : base("Slow") { }
    }
}
