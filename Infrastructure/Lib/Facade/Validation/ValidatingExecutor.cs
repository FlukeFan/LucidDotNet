using System.Threading.Tasks;

namespace Lucid.Infrastructure.Lib.Facade.Validation
{
    public class ValidatingExecutor : IExecutor
    {
        private IExecutor _inner;

        public ValidatingExecutor(IExecutor inner)
        {
            _inner = inner;
        }

        Task<object> IExecutor.Execute(object executable)
        {
            ExecutableValidator.Validate(executable);
            return _inner.Execute(executable);
        }
    }
}
