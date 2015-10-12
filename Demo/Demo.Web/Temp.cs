using Demo.Domain.Utility;
using Lucid.Infrastructure.Persistence.NHibernate;

namespace Lucid.Demo.Web
{
    /// <summary>
    /// Temporary class just to start compilation
    /// </summary>
    public static class Temp
    {
        public static void NotCalled()
        {
            Mapping.CreateMappings<int>(typeof(DemoEntity));
        }
    }
}