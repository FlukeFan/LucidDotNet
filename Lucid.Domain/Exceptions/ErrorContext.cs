using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Lucid.Domain.Exceptions
{
    public class ErrorContext
    {
        public bool     IsLogicException;
        public string   Type;
        public string   StackTrace;

        public IList<string>                        Messages            = new List<string>();
        public IDictionary<string, IList<string>>   PropertyMessages    = new Dictionary<string, IList<string>>();

        public ErrorContext() { }

        public string FormatMessage()
        {
            var message = IsLogicException ? "Logic exception:\n" : "Non logic exception:";
            message += string.Join("\n", Messages);
            return message;
        }

        internal ErrorContext AddMessage(string message)
        {
            Messages.Add(message);
            return this;
        }

        public void Add(ValidationResult result)
        {
            foreach (var memberName in result.MemberNames)
            {
                if (!PropertyMessages.ContainsKey(memberName))
                    PropertyMessages[memberName] = new List<string>();

                PropertyMessages[memberName].Add(result.ErrorMessage);
            }
        }
    }
}
