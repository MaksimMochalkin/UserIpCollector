namespace UserIpCollector.Services
{
    using Microsoft.Extensions.Caching.Distributed;
    using Newtonsoft.Json;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using UserIpCollector.Abstractions.Interfaces;
    using UserIpCollector.Data.Entities;

    public class UserIpAdresesCacheService : IUserIpAdresesCacheService
    {
        private readonly IDistributedCache _cache;
        private readonly IRepositoryManager _repositoryManager;
        private readonly ILogger<UserIpAdresesCacheService> _logger;

        public UserIpAdresesCacheService(
            IDistributedCache cache,
            IRepositoryManager repositoryManager,
            ILogger<UserIpAdresesCacheService> logger)
        {
            _cache = cache;
            _repositoryManager = repositoryManager;
            _logger = logger;
        }

        public async Task<List<User>> FindUsersByIpPrefixAsync(string ipPrefix)
        {
            var cacheKey = $"users:byIp:{ipPrefix}";
            _logger.LogTrace($"Searching user's ips started by key {cacheKey}");
            var cachedUsers = await _cache.GetStringAsync(cacheKey);

            if (!string.IsNullOrEmpty(cachedUsers))
            {
                return JsonConvert.DeserializeObject<List<User>>(cachedUsers);
            }

            var users = await _repositoryManager.UserRepository.FindUsersByPartialIpAsync(ipPrefix);

            await _cache.SetStringAsync(cacheKey, JsonConvert.SerializeObject(users),
                new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1) });

            return users;
        }

        public async Task AddUserIpsAsync(User user)
        {
            var key = $"user:ips:{user.Id}";
            _logger.LogTrace($"Adding user and his IP to cache has started by key {key}");

            var userIps = await GetCacheAsync<List<UserIpAddress>>(key) ?? new List<UserIpAddress>();

            userIps.AddRange(user.IpAddresses);

            await SetCacheAsync(key, userIps);
        }

        public async Task<List<UserIpAddress>> GetAllIpsByUserIdAsync(long userId)
        {
            var key = $"alluser:ips:{userId}";
            var cached = await GetCacheAsync<List<UserIpAddress>>(key) ?? new List<UserIpAddress>();
            if (!cached.Any())
            {
                var userIps = await _repositoryManager.UserIpAddressRepository.GetAllIpsByUserIdAsync(userId);
                await SetCacheAsync(key, userIps);
                return userIps;
            }

            return cached;
        }

        private async Task SetCacheAsync<T>(string key, T value, TimeSpan? expiration = null)
        {
            var serialized = JsonConvert.SerializeObject(value);
            await _cache.SetStringAsync(key, serialized, new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = expiration ?? TimeSpan.FromHours(1)
            });
            _logger.LogTrace($"Value: {serialized} added to cache");
        }

        private async Task<T?> GetCacheAsync<T>(string key)
        {
            var cached = await _cache.GetStringAsync(key);
            var result = cached == null ? default : JsonConvert.DeserializeObject<T>(cached);
            _logger.LogTrace($"Cacheed result {result}");

            return result;
        }
    }
}
