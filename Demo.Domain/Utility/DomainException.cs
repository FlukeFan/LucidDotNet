using Lucid.Facade.Exceptions;

namespace Demo.Domain.Utility
{
    public class DomainException : LucidException
    {
        public DomainException(string message) : base(message) { }
    }
}
