using System.IO;
using System.Threading.Tasks;
using Lucid.Infrastructure.Lib.Facade;
using Lucid.Infrastructure.Lib.Facade.Exceptions;

namespace Lucid.Modules.ProjectCreation
{
    public class GenerateProject : Command<byte[]>
    {
        public string Name { get; set; } = "Demo";

        public override Task<byte[]> Execute()
        {
            ValidateName();

            var assembly = GetType().Assembly;

            using (var zipInputStream = assembly.GetManifestResourceStream("Lucid.Modules.ProjectCreation.Project.zip"))
            using (var zipOutputStream = Generate.Project(zipInputStream, Name))
                return Task.FromResult(StreamToBytes(zipOutputStream));
        }

        private static byte[] StreamToBytes(Stream stream)
        {
            using (var memory = new MemoryStream())
            {
                stream.CopyTo(memory);
                return memory.ToArray();
            }
        }

        private void ValidateName()
        {
            ValidateStartsWithAlpha();
            ValidateNoSpaces();
            ValidateNoSpecialCharacters();
        }

        private void ValidateStartsWithAlpha()
        {
            if (!char.IsLetter(Name[0]))
                throw this.PropertyException(gp => gp.Name, p => "Please supply a Name that starts with a letter");
        }

        private void ValidateNoSpaces()
        {
            if (Name.Contains(" ") == true)
                throw this.PropertyException(gp => gp.Name, p => "Please supply a Name that does not contains spaces");
        }

        private void ValidateNoSpecialCharacters()
        {
            foreach (var chr in Name)
                if (!char.IsLetterOrDigit(chr) && chr != '_')
                    throw this.PropertyException(gp => gp.Name, p => "Please supply a Name that does not contains special characters");
        }
    }
}
