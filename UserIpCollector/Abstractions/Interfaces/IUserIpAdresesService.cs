namespace UserIpCollector.Abstractions.Interfaces
{
    using UserIpCollector.Data.Entities;

    public interface IUserIpAdresesService
    {
        Task<List<User>> FindUsersByIpPrefixAsync(string ipRange);
        Task AddUserWithIpAsync(User user);
        Task<List<UserIpAddress>> GetUserIpsAsync(long userId);
    }
}
