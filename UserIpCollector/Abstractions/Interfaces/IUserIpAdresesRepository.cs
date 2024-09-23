namespace UserIpCollector.Abstractions.Interfaces
{
    using UserIpCollector.Data.Entities;

    public interface IUserIpAdresesRepository : IBaseRepository<UserIpAddress>
    {
        Task<List<UserIpAddress>> GetAllIpsByUserIdAsync(long userId);
        Task<UserIpAddress?> GetLastConnectionByUserIdAsync(long userId);
    }
}
