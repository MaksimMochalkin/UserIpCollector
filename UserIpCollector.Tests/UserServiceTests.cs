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

    public class UserServiceTests
    {
        private readonly DbHelper _dbHelper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepositoryManager _repositoryManager;
        private readonly IServiceManager _serviceManager;

        public UserServiceTests()
        {
            _dbHelper = new DbHelper();
            var context = _dbHelper.GetInMemoryDbContext();
            var memoryCache = new MemoryDistributedCache(new OptionsWrapper<MemoryDistributedCacheOptions>(new MemoryDistributedCacheOptions()));
            var factory = new NullLoggerFactory();
            _repositoryManager = new RepositoryManager(context, factory);
            _unitOfWork = new UnitOfWork(context);
            _serviceManager = new ServiceManager(_repositoryManager, _unitOfWork, memoryCache, factory);
        }

        [Fact]
        public async Task CreateUserWithIp_ShouldCreateUserAndIp()
        {
            // Arrange
            var userId = 10L;
            var ipAddress = "192.168.0.1";

            // Act
            var createdUser = await _serviceManager.UserService.CreateUserWithIpAsync(userId, ipAddress);

            // Assert
            var userIp = createdUser.IpAddresses.FirstOrDefault();
            createdUser.ShouldNotBeNull();
            userId.ShouldBe(createdUser.Id);
            userIp.ShouldNotBeNull();
            ipAddress.ShouldBe(userIp.IpAddress);
            _dbHelper.EnsureDeleted();
        }

        [Fact]
        public async Task GetAllUserIpAddresses_ShouldReturnUserIps()
        {
            // Arrange
            _dbHelper.SeedData();

            // Act
            var lastConnection = await _serviceManager.UserService.GetLastConnectionAsync(1);

            // Assert
            lastConnection.ShouldNotBeNull();
            lastConnection.Id.ShouldBe(3);
            lastConnection.IpAddress.ShouldBe("192.168.0.3");
            _dbHelper.EnsureDeleted();
        }
    }
}