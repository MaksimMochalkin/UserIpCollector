namespace UserIpCollector.Tests
{
    using Microsoft.Extensions.Caching.Distributed;
    using Microsoft.Extensions.Caching.Memory;
    using Microsoft.Extensions.Logging.Abstractions;
    using Microsoft.Extensions.Options;
    using Shouldly;
    using UserIpCollector.Abstractions.Interfaces;
    using UserIpCollector.Data.Entities;
    using UserIpCollector.Managers;
    using UserIpCollector.Repositories;
    using UserIpCollector.Tests.Helpers;

    public class UserIpAdresesCacheServiceTests
    {
        private readonly DbHelper _dbHelper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepositoryManager _repositoryManager;
        private readonly IServiceManager _serviceManager;

        public UserIpAdresesCacheServiceTests()
        {
            _dbHelper = new DbHelper();
            var context = _dbHelper.GetInMemoryDbContext();
            var memoryCache = new MemoryDistributedCache(new OptionsWrapper<MemoryDistributedCacheOptions>(new MemoryDistributedCacheOptions()));
            var factory = new NullLoggerFactory();
            _repositoryManager = new RepositoryManager(context, factory);
            _unitOfWork = new UnitOfWork(context);
            _serviceManager = new ServiceManager(_repositoryManager, _unitOfWork, memoryCache, factory);
        }

        [Theory]
        [InlineData("192.168", 3)]
        [InlineData("0.2", 2)]
        [InlineData(".168", 3)]
        public async Task FindUsersByIpPrefix_ShouldReturnUsersFromCache(string ipPrefix, int userCount)
        {
            // Arrange
            _dbHelper.SeedData();

            // Act
            var result = await _serviceManager.UserIpAdresesCacheService.FindUsersByIpPrefixAsync(ipPrefix);
            var cachedResult = await _serviceManager.UserIpAdresesCacheService.GetAllUsersByIpPrefixKeyFromCacheAsync($"users:byIp:{ipPrefix}");

            // Assert
            result.ShouldNotBeNull();
            result.Count.ShouldBe(userCount);
            cachedResult.ShouldNotBeNull();
            cachedResult.Count.ShouldBe(userCount);
            _dbHelper.EnsureDeleted();
        }

        [Fact]
        public async Task GetUserIps_ShouldReturnIpsFromCache()
        {
            // Arrange
            _dbHelper.SeedData();

            // Act
            var result = await _serviceManager.UserIpAdresesCacheService.GetAllIpsByUserIdAsync(1);
            var cachedResult = await _serviceManager.UserIpAdresesCacheService.GetIpsByKeyFromCacheAsync($"alluser:ips:{1}");
            // Assert
            result.ShouldNotBeNull();
            result.Count.ShouldBe(3);
            cachedResult.ShouldNotBeNull();
            cachedResult.Count.ShouldBe(3);
            _dbHelper.EnsureDeleted();
        }
    }
}
