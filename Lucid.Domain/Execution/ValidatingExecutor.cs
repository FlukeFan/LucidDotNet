namespace Lucid.Domain.Execution
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
            Validator.Validate(executable);
            return _inner.Execute(executable);
        }
    }
}
