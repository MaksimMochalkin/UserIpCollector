namespace UserIpCollector.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using UserIpCollector.Abstractions.Interfaces;
    using UserIpCollector.Data.Entities;
    using UserIpCollector.Managers;

    public class UserService : IUserService
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<UserService> _logger;

        public UserService(
            IRepositoryManager repositoryManager,
            IUnitOfWork unitOfWork,
            ILogger<UserService> logger)
        {
            _repositoryManager = repositoryManager;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<User> CreateUserWithIpAsync(long userId, string ipAddress)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();
                var user = await _repositoryManager.UserRepository.GetByIdAsync(userId);
                if (user == null)
                {
                    _logger.LogTrace("User not found. Create a new user.");
                    user = new User { Id = userId };
                    await _repositoryManager.UserRepository.AddAsync(user);
                }

                var userIp = new UserIpAddress
                {
                    IpAddress = ipAddress,
                    ConnectedAt = DateTime.UtcNow,
                    UserId = userId
                };

                await _repositoryManager.UserIpAddressRepository.AddAsync(userIp);
                await _unitOfWork.CommitAsync();

                _logger.LogTrace("User with ip's successfully created.");

                return user;
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task<List<User>> FindUsersByPartialIpAsync(string partialIp)
        {
            return await _repositoryManager.UserRepository.FindUsersByPartialIpAsync(partialIp); ;
        }

        public async Task<List<UserIpAddress>> GetAllUserIpAddressesAsync(long userId)
        {
            return await _repositoryManager.UserIpAddressRepository.GetAllIpsByUserIdAsync(userId);
        }

        public async Task<UserIpAddress?> GetLastConnectionAsync(long userId)
        {
            return await _repositoryManager
                .UserIpAddressRepository
                .GetLastConnectionByUserIdAsync(userId); ;
        }
    }
}
