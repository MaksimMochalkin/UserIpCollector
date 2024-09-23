namespace UserIpCollector.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using System.ComponentModel.DataAnnotations;
    using UserIpCollector.Abstractions.Interfaces;
    using UserIpCollector.Requests;

    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;
        private readonly ILogger<UserController> _logger;

        public UserController(
            IServiceManager serviceManager,
            ILogger<UserController> logger)
        {
            _serviceManager = serviceManager;
            _logger = logger;
        }

        [HttpPost("init-user-connection")]
        public async Task<IActionResult> InitUserConnection([FromBody] ConnectionRequest request)
        {
            _logger.LogTrace("InitUserConnection started");
            var user = await _serviceManager.UserService.CreateUserWithIpAsync(request.UserId, request.IpAddress);

            await _serviceManager.UserIpAdresesCacheService.AddUserIpsAsync(user);
            return Ok("User with IP address created.");
        }

        [HttpGet("find-by-ip")]
        public async Task<IActionResult> FindUsersByIpPrefix(
            [FromQuery][Required] string ipPrefix)
        {
            _logger.LogTrace("FindUsersByIpPrefix started");
            var users = await _serviceManager.UserIpAdresesCacheService.FindUsersByIpPrefixAsync(ipPrefix);
            _logger.LogTrace("FindUsersByIpPrefix finished");

            return Ok(users);
        }

        [HttpGet("{userId}/ips")]
        public async Task<IActionResult> GetUserIpAddresses(
            [Required] long userId)
        {
            _logger.LogTrace("GetUserIpAddresses started");
            var ips = await _serviceManager.UserIpAdresesCacheService.GetAllIpsByUserIdAsync(userId);
            _logger.LogTrace("GetUserIpAddresses finished");

            return Ok(ips);
        }

        [HttpGet("{userId}/last-connection")]
        public async Task<IActionResult> GetLastConnection(
            [Required] long userId)
        {
            _logger.LogTrace("GetLastConnection started");
            var lastConnection = await _serviceManager.UserService.GetLastConnectionAsync(userId);
            _logger.LogTrace("GetLastConnection finished");

            return Ok(lastConnection);
        }
    }
}
