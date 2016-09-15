using Lucid.Facade.Exceptions;

namespace Demo.Domain.Utility
{
    public class DemoException : FacadeException
    {
        public DemoException(string message) : base(message) { }
    }
}
