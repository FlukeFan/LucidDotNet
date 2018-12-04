using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Lucid.Infrastructure.Lib.Facade;

namespace Lucid.Modules.ProjectCreation
{
    public class ExecutableValidator
    {
        public static void Validate(object command)
        {
        }
    }

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
                throw this.PropertyError(gp => gp.Name, p => "Please supply a Name that starts with a letter");
        }

        private void ValidateNoSpaces()
        {
            if (Name.Contains(" ") == true)
                throw this.PropertyError(gp => gp.Name, p => "Please supply a Name that does not contains spaces");
        }

        private void ValidateNoSpecialCharacters()
        {
            foreach (var chr in Name)
                if (!char.IsLetterOrDigit(chr) && chr != '_')
                    throw this.PropertyError(gp => gp.Name, p => "Please supply a Name that does not contains special characters");
        }
    }

    public static class FacadeExceptionExtensions
    {
        public static FacadeException PropertyError<T>(this T source, Expression<Func<T, object>> property, Func<string, string> message)
        {
            var propertyName = ((MemberExpression)property.Body).Member.Name;
            var validationResult = new ValidationResult(message(propertyName), new string[] { propertyName });
            return new FacadeException(new ValidationResult[] { validationResult });
        }
    }
}
