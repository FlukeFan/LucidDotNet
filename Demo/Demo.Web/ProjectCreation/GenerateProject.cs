using System.IO;
using Lucid.Domain.Execution;

namespace Demo.Web.ProjectCreation
{
    public class GenerateProject : Command<byte[]>
    {
        public string Name { get; set; }

        public override byte[] Execute()
        {
            var assembly = GetType().Assembly;
            using (var zip = assembly.GetManifestResourceStream("Demo.Web.Project.zip"))
                return StreamToBytes(zip);
        }

        private static byte[] StreamToBytes(Stream stream)
        {
            return null;
        }
    }
}