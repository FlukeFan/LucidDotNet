namespace Lucid.Persistence
{
    public interface IEntity<TId>
    {
        TId Id { get; }
    }
}
