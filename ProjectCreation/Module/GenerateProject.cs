using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Linq.Expressions;

namespace Lucid.ProjectCreation
{
    public class ExecutableValidator
    {
        public static void Validate(object command)
        {
        }
    }

    public class FacadeException : Exception
    {
        public IEnumerable<string>                  Messages            { get; protected set; }
        public IDictionary<string, IList<string>>   PropertyMessages    { get; protected set; }

        public FacadeException(IEnumerable<string> messages, IDictionary<string, IList<string>> propertyMessages)
            : base(FormatMessage(messages, propertyMessages))
        {
            Messages = messages;
            PropertyMessages = propertyMessages;
        }

        public FacadeException(IEnumerable<ValidationResult> validationResults)
            : this(new List<string>(), ConvertResults(validationResults))
        { }

        public static string FormatMessage(IEnumerable<string> messages, IDictionary<string, IList<string>> propertyMessages)
        {
            var lines = new List<string>(messages);

            foreach (var kvp in propertyMessages)
                lines.AddRange(kvp.Value.Select(s => kvp.Key + ": " + s));

            return string.Join("\n", lines);
        }

        public static IDictionary<string, IList<string>> ConvertResults(IEnumerable<ValidationResult> validationResults)
        {
            var results = new Dictionary<string, IList<string>>();

            foreach (var result in validationResults)
                foreach (var propertyName in result.MemberNames)
                {
                    if (!results.ContainsKey(propertyName))
                        results[propertyName] = new List<string>();

                    results[propertyName].Add(result.ErrorMessage);
                }

            return results;
        }
    }

    public class GenerateProject
    {
        public string Name { get; set; }

        public byte[] Execute()
        {
            ValidateName();

            var assembly = GetType().Assembly;

            using (var zipInputStream = assembly.GetManifestResourceStream("Lucid.ProjectCreation.Project.zip"))
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
