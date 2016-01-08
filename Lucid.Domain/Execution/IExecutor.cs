namespace Lucid.Domain.Execution
{
    public interface IExecutor
    {
        object Execute(object executable);
    }
}
