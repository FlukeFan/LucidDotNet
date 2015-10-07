namespace Lucid.Domain.Persistence
{
    public interface IEntity<TId>
    {
        TId Id { get; }
    }
}
