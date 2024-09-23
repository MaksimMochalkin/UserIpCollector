namespace UserIpCollector.Abstractions.Interfaces
{
    using UserIpCollector.Data.Entities;

    public interface IUserRepository : IBaseRepository<User>
    {
        Task<List<User>> FindUsersByPartialIpAsync(string partialIp);
    }
}
