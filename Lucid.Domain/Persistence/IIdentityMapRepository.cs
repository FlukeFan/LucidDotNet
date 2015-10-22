namespace Lucid.Domain.Persistence
{
    public interface IIdentityMapRepository<TId> : IRepository<TId>
    {
        void Clear();
    }
}
