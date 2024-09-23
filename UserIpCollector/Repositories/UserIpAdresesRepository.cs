namespace UserIpCollector.Repositories
{
    using Microsoft.EntityFrameworkCore;
    using UserIpCollector.Abstractions.Interfaces;
    using UserIpCollector.Data;
    using UserIpCollector.Data.Entities;

    public class UserIpAdresesRepository : BaseRepository<UserIpAddress>, IUserIpAdresesRepository
    {
        private readonly ILogger<UserIpAdresesRepository> _logger;

        public UserIpAdresesRepository(ApplicationDbContext context,
            ILogger<UserIpAdresesRepository> logger)
            : base(context)
        {
            _logger = logger;
        }
        
        public async Task<List<UserIpAddress>> GetAllIpsByUserIdAsync(long userId)
        {
            _logger.LogTrace("Getting all user IP addresses from the database");

            var ips = await _context.UserIpAddresses
                .Where(ip => ip.UserId == userId)
                .Distinct()
                .ToListAsync();

            if (!ips.Any())
            {
                throw new Exception("No ip's found for this user.");
            }

            _logger.LogTrace("Getting all user IP addresses from the database successfully finished");

            return ips;
        }

        public async Task<UserIpAddress?> GetLastConnectionByUserIdAsync(long userId)
        {
            _logger.LogTrace("Search for all user IP addresses started in the database");

            var lastConnection = await _context.UserIpAddresses
                .Where(ip => ip.UserId == userId)
                .OrderByDescending(ip => ip.ConnectedAt)
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
