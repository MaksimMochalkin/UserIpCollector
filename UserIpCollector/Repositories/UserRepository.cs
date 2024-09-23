namespace UserIpCollector.Repositories
{
    using Microsoft.EntityFrameworkCore;
    using UserIpCollector.Abstractions.Interfaces;
    using UserIpCollector.Data;
    using UserIpCollector.Data.Entities;

    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        private readonly ILogger<UserRepository> _logger;

        public UserRepository(ApplicationDbContext context,
            ILogger<UserRepository> logger)
            : base(context) 
        {
            _logger = logger;
        }

        public async Task<List<User>> FindUsersByPartialIpAsync(string partialIp)
        {
            _logger.LogTrace("The search for users by part of the IP address in the database has begun");

            var users = await _context.UserIpAddresses
                .Where(ip => EF.Functions.Like(ip.IpAddress, $"%{partialIp}%"))
                .Select(ip => ip.User)
                .Distinct()
                .ToListAsync();

            if (!users.Any())
            {
                throw new Exception("No users found by this prefix.");
            }

            _logger.LogTrace("The search for users by part of the IP address in the database successfully finished");

            return users;
        }
    }
}
