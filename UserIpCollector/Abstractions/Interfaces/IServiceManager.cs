namespace UserIpCollector.Abstractions.Interfaces
{
    public interface IServiceManager
    {
        public IUserService UserService { get; }
        public IUserIpAdresesCacheService UserIpAdresesCacheService { get; }
    }
}
