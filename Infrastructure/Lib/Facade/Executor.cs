namespace Lucid.Infrastructure.Lib.Facade
{
    public class Executor : IExecutor
    {
        object IExecutor.Execute(IExecutable executable)
        {
            return executable.Execute();
        }
    }
}
