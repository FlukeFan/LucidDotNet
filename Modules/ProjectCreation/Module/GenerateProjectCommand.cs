﻿using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Threading.Tasks;
using Lucid.Lib.Facade;
using Lucid.Lib.Facade.Exceptions;
using Lucid.Lib.Facade.Validation;

namespace Lucid.Modules.ProjectCreation
{
    public class GenerateProjectCommand : CommandAsync<byte[]>, ICustomValidation
    {
        [Required(ErrorMessage = "Please supply a Name")]
        public string Name { get; set; } = "Demo";

        public override Task<byte[]> ExecuteAsync()
        {
            var assembly = GetType().Assembly;

            using (var zipInputStream = assembly.GetManifestResourceStream("Lucid.Modules.ProjectCreation.Project.zip"))
            using (var zipOutputStream = ZipGenerator.Generate(zipInputStream, Name))
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

        void ICustomValidation.Validate()
        {
            ValidateName();
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
