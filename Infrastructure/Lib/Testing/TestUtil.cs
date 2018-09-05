using System.IO;

namespace Lucid.Infrastructure.Lib.Testing
{
    public class TestUtil
    {
        public static string ProjectPath()
        {
            var cd = Path.GetFullPath(".");

            while (Directory.GetFiles(cd, "*.csproj").Length == 0)
            {
                var parent = Directory.GetParent(cd)?.FullName;

                if (parent == cd || parent == null)
                    throw new System.Exception($".csproj not found in parent of {Path.GetFullPath(".")}");

                cd = parent;
            }

            return cd;
        }
    }
}
