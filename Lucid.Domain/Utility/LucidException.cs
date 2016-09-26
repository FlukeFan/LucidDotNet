using SimpleFacade.Exceptions;

namespace Lucid.Domain.Utility
{
    public class LucidException : FacadeException
    {
        public LucidException(string message) : base(message) { }
    }
}
