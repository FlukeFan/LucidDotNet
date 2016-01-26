using Lucid.Domain.Execution;

namespace Lucid.Domain.Validation
{
    public class ValidatingExecutor : IExecutor
    {
        private IExecutor _inner;

        public ValidatingExecutor(IExecutor inner)
        {
            _inner = inner;
        }

        object IExecutor.Execute(object executable)
        {
            LucidValidator.Validate(executable);
            return _inner.Execute(executable);
        }
    }
}
