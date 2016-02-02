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

            using (var zipInputStream = assembly.GetManifestResourceStream("Demo.Web.Project.zip"))
            using (var zipOutputStream = Generate.Project(zipInputStream, Name))
                return StreamToBytes(zipOutputStream);
        }

        private static byte[] StreamToBytes(Stream stream)
        {
            using (var memory = new MemoryStream())
            {
                stream.CopyTo(memory);
                return memory.ToArray();
            }
        }
    }
}