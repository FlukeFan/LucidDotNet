namespace Lucid.Persistence
{
    public interface IIdentityMapRepository<TId> : IRepository<TId>
    {
        void Clear();
    }
}
