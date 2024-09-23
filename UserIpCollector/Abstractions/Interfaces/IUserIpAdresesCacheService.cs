namespace UserIpCollector.Abstractions.Interfaces
{
    using UserIpCollector.Data.Entities;

    public interface IUserIpAdresesCacheService
    {
        Task<List<User>> FindUsersByIpPrefixAsync(string ipRange);
        Task AddUserIpsAsync(User user);
        Task<List<UserIpAddress>> GetAllIpsByUserIdAsync(long userId);
        Task<List<UserIpAddress>> GetIpsByKeyFromCacheAsync(string key);
        Task<User?> GetUserByKeyFromCacheAsync(string key);
        Task<List<User>?> GetAllUsersByIpPrefixKeyFromCacheAsync(string key);
    }
}
