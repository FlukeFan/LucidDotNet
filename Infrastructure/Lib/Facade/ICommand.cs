namespace Lucid.Infrastructure.Lib.Facade
{
    public interface ICommand<T>
    {
        T Execute();
    }
}
