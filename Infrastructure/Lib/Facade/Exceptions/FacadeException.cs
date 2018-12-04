using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Lucid.Infrastructure.Lib.Facade.Exceptions
{
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
}
