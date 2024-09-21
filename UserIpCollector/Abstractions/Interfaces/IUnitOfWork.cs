namespace UserIpCollector.Abstractions.Interfaces
{
    public interface IUnitOfWork
    {
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        Task RollBackChangesAsync(CancellationToken cancellationToken = default);
    }
}
