namespace UserIpCollector.Tests.Helpers
{
    using Microsoft.EntityFrameworkCore;
    using System;
    using UserIpCollector.Data;
    using UserIpCollector.Data.Entities;

    public class DbHelper
    {
        private ApplicationDbContext _context;

        public ApplicationDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlite("DataSource=:memory:")
                .Options;
            
            _context = new ApplicationDbContext(options);
            _context.Database.OpenConnection();
            _context.Database.EnsureCreated();
            return _context;
        }

        public void EnsureDeleted()
        {
            _context.Database.EnsureDeleted();
        }

        public void SeedData()
        {
            var users = new List<User>
            {
                new User
                {
                    Id = 1,
                    IpAddresses = new List<UserIpAddress>()
                },
                new User
                {
                    Id = 2,
                    IpAddresses = new List<UserIpAddress>()
                },
                new User
                {
                    Id = 3,
                    IpAddresses = new List<UserIpAddress>()
                },
            };

            _context.Users.AddRange(users);

            var addresses = new List<UserIpAddress>
            {
                new UserIpAddress
                {
                    Id = 1,
                    UserId = 1,
                    IpAddress = "192.168.0.1",
                    ConnectedAt = DateTime.UtcNow,
                },
                new UserIpAddress
                {
                    Id = 2,
                    UserId = 1,
                    IpAddress = "192.168.0.2",
                    ConnectedAt = DateTime.UtcNow.AddMilliseconds(2000),
                },
                new UserIpAddress
                {
                    Id = 3,
                    UserId = 1,
                    IpAddress = "192.168.0.3",
                    ConnectedAt = DateTime.UtcNow.AddMilliseconds(4000),
                },
                new UserIpAddress
                {
                    Id = 4,
                    UserId = 2,
                    IpAddress = "192.168.0.1",
                    ConnectedAt = DateTime.UtcNow.AddMilliseconds(4000),
                },
                new UserIpAddress
                {
                    Id = 5,
                    UserId = 2,
                    IpAddress = "192.168.0.4",
                    ConnectedAt = DateTime.UtcNow.AddMilliseconds(8000),
                },
                new UserIpAddress
                {
                    Id = 6,
                    UserId = 3,
                    IpAddress = "192.168.0.5",
                    ConnectedAt = DateTime.UtcNow.AddMilliseconds(4000),
                },
                new UserIpAddress
                {
                    Id = 7,
                    UserId = 3,
                    IpAddress = "192.168.0.6",
                    ConnectedAt = DateTime.UtcNow,
                },
                new UserIpAddress
                {
                    Id = 8,
                    UserId = 3,
                    IpAddress = "192.168.0.2",
                    ConnectedAt = DateTime.UtcNow.AddMilliseconds(2000),
                },
                new UserIpAddress
                {
                    Id = 9,
                    UserId = 3,
                    IpAddress = "192.168.0.7",
                    ConnectedAt = DateTime.UtcNow.AddMilliseconds(3000),
                }
            };

            _context.UserIpAddresses.AddRange(addresses);
            _context.SaveChanges();
        }
    }
}
