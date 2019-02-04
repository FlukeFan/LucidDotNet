using System.Threading.Tasks;

namespace Lucid.Infrastructure.Lib.Facade.Validation
{
    public class ValidatingExecutorAsync : IExecutorAsync
    {
        private IExecutorAsync _inner;

        public ValidatingExecutorAsync(IExecutorAsync inner)
        {
            _inner = inner;
        }

        Task<object> IExecutorAsync.ExecuteAsync(IExecutionContext context)
        {
            ExecutableValidator.Validate(context.Executable);
            return _inner.ExecuteAsync(context);
        }
    }
}
