namespace UserIpCollector.Abstractions.Interfaces
{
    using UserIpCollector.Data.Entities;

    public interface IUserService
    {
        Task<User> CreateUserWithIp(long userId, string ipAddress);
        Task<List<UserIpAddress>> GetAllUserIpAddressesAsync(long userId);
        Task<UserIpAddress> GetLastConnectionAsync(long userId);
        Task<List<User>> FindUsersByPartialIpAsync(string partialIp);
    }
}
