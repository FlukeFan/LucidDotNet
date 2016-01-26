using Lucid.Domain.Exceptions;

namespace Demo.Domain.Utility
{
    public class DomainException : LucidException
    {
        public DomainException(string message) : base(message) { }
    }
}
