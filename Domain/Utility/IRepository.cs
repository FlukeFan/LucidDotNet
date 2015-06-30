
namespace Lucid.Domain.Utility
{
    public interface IRepository
    {
        T Save<T>(T entity) where T : Entity;
    }
}
