using Lucid.Facade.Execution;

namespace Lucid.Facade.Validation
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
            ExecutableValidator.Validate(executable);
            return _inner.Execute(executable);
        }
    }
}
