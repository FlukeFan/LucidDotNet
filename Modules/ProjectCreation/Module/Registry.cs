using System.Threading.Tasks;

namespace Lucid.Modules.ProjectCreation
{
    public interface IExecutable
    {
        object Execute();
    }

    public interface ICommand<T>
    {
        T Execute();
    }

    public abstract class Command<T> : ICommand<T>, IExecutable
    {
        public abstract T Execute();

        object IExecutable.Execute()
        {
            return Execute();
        }
    }

    public interface IExecutor
    {
        object Execute(IExecutable executable);
    }

    public class Executor : IExecutor
    {
        object IExecutor.Execute(IExecutable executable)
        {
            return executable.Execute();
        }
    }

    public static class Registry
    {
        public static IExecutor Executor { get; set; }

        public static async Task StartupAsync()
        {
            await Task.Run(() =>
            {
                Executor = new Executor();
            });
        }
    }
}
