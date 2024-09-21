namespace UserIpCollector.Services
{
    using Microsoft.Extensions.Caching.Distributed;
    using UserIpCollector.Abstractions.Interfaces;
    using UserIpCollector.Data;

    public class ServiceManager : IServiceManager
    {
        private readonly Lazy<IUserService> _userService;
        private readonly Lazy<IUserIpAdresesService> _ipAddresessService;

        public ServiceManager(ApplicationDbContext context,
            IDistributedCache cache)
        {
            //_userService = new Lazy<IUserService> (() => new UserService(context, cache));
        }

        public IUserService UserService => _userService.Value;

        public IUserIpAdresesService UserIpAdresesService => _ipAddresessService.Value;
    }
}
