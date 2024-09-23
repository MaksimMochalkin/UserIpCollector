namespace UserIpCollector.Managers
{
    using UserIpCollector.Abstractions.Interfaces;
    using UserIpCollector.Data;
    using UserIpCollector.Repositories;

    public class RepositoryManager : IRepositoryManager
    {
        private readonly ApplicationDbContext _context;
        private readonly Lazy<IUserRepository> _userRepository;
        private readonly Lazy<IUserIpAdresesRepository> _userIpAddressRepository;

        public RepositoryManager(ApplicationDbContext context,
            ILoggerFactory loggerFactory)
        {
            _context = context;

            _userRepository = new Lazy<IUserRepository>(() => new UserRepository(_context,
                loggerFactory.CreateLogger<UserRepository>()));
            _userIpAddressRepository = new Lazy<IUserIpAdresesRepository>(() => new UserIpAdresesRepository(_context,
                loggerFactory.CreateLogger<UserIpAdresesRepository>()));
        }

        public IUserRepository UserRepository => _userRepository.Value;
        public IUserIpAdresesRepository UserIpAddressRepository => _userIpAddressRepository.Value;

    }
}
