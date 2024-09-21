namespace UserIpCollector.Services
{
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using UserIpCollector.Abstractions.Interfaces;
    using UserIpCollector.Data;
    using UserIpCollector.Data.Entities;

    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<UserService> _logger;

        public UserService(
            ApplicationDbContext dbContext,
            ILogger<UserService> logger)
        {
            _context = dbContext;
            _logger = logger;
        }

        public async Task<User> CreateUserWithIp(long userId, string ipAddress)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                _logger.LogTrace("User not found. Create a new user.");
                user = new User { Id = userId };
                _context.Users.Add(user);
            }

            var userIp = new UserIpAddress
            {
                IpAddress = ipAddress,
                ConnectedAt = DateTime.UtcNow,
                User = user
            };

            _context.UserIpAddresses.Add(userIp);
            await _context.SaveChangesAsync();
            _logger.LogTrace("User with ip's successfully created.");

            return user;
        }

        public async Task<List<User>> FindUsersByPartialIpAsync(string partialIp)
        {
            _logger.LogTrace("The search for users by part of the IP address in the database has begun");
            var users = await _context.UserIpAddresses
            .Where(address => EF.Functions.Like(address.IpAddress, $"%{partialIp}%"))
            .Select(user => user.User)
            .Distinct()
            .ToListAsync();

            if (users.Any())
            {
                throw new Exception("No users found by this prefix.");
            }

            _logger.LogTrace("The search for users by part of the IP address in the database successfully finished");

            return users;
        }

        public async Task<List<UserIpAddress>> GetAllUserIpAddressesAsync(long userId)
        {
            _logger.LogTrace("Getting all user IP addresses from the database");

            var ips = await _context.UserIpAddresses
            .Where(address => address.UserId == userId)
            .Distinct()
            .ToListAsync();

            if (ips.Any())
            {
                throw new Exception("No ip's found for this user.");
            }
            _logger.LogTrace("Getting all user IP addresses from the database successfully finished");

            return ips;
        }

        public async Task<UserIpAddress> GetLastConnectionAsync(long userId)
        {
            _logger.LogTrace("Search for all user IP addresses started in the database");

            var lastConnection = await _context.UserIpAddresses
            .Where(address => address.UserId == userId)
            .OrderByDescending(address => address.ConnectedAt)
            .FirstOrDefaultAsync();

            if (lastConnection == null)
            {
                throw new Exception("No connections found for this user.");
            }
            _logger.LogTrace("Search for all user IP addresses in the database successfully finished");

            return lastConnection;
        }
    }
}
