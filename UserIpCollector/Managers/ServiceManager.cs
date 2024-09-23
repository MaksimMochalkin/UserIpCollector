namespace UserIpCollector.Managers
{
    using Microsoft.Extensions.Caching.Distributed;
    using UserIpCollector.Abstractions.Interfaces;
    using UserIpCollector.Services;

    public class ServiceManager : IServiceManager
    {
        private readonly Lazy<IUserService> _userService;
        private readonly Lazy<IUserIpAdresesCacheService> _ipAddresessService;

        public ServiceManager(IRepositoryManager repositoryManager,
            IUnitOfWork unitOfWork,
            IDistributedCache cache,
            ILoggerFactory loggerFactory)
        {
            _userService = new Lazy<IUserService>(() => new UserService(
                repositoryManager,
                unitOfWork,
                loggerFactory.CreateLogger<UserService>()));

            _ipAddresessService = new Lazy<IUserIpAdresesCacheService>(() => new UserIpAdresesCacheService(
                cache, repositoryManager,
                loggerFactory.CreateLogger<UserIpAdresesCacheService>()));
        }

        public IUserService UserService => _userService.Value;

        public IUserIpAdresesCacheService UserIpAdresesCacheService => _ipAddresessService.Value;
    }
}
