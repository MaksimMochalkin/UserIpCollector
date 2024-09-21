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
        private readonly IUserService _userService;
        private readonly IUserIpAdresesService _userIpAdresesService;
        private readonly ILogger<UserController> _logger;

        public UserController(
            IUserService userService,
            IUserIpAdresesService userIpAdresesService,
            ILogger<UserController> logger)
        {
            _userService = userService;
            _userIpAdresesService = userIpAdresesService;
            _logger = logger;
        }

        [HttpPost("init-user-connection")]
        public async Task<IActionResult> InitUserConnection([FromBody] ConnectionRequest request)
        {
            _logger.LogTrace("InitUserConnection started");
            var user = await _userService.CreateUserWithIp(request.UserId, request.IpAddress);

            await _userIpAdresesService.AddUserWithIpAsync(user);
            return Ok("User with IP address created.");
        }

        [HttpGet("find-by-ip")]
        public async Task<IActionResult> FindUsersByIpPrefix(
            [FromQuery][Required] string ipPrefix)
        {
            _logger.LogTrace("FindUsersByIpPrefix started");
            var users = await _userIpAdresesService.FindUsersByIpPrefixAsync(ipPrefix);
            _logger.LogTrace("FindUsersByIpPrefix finished");

            return Ok(users);
        }

        [HttpGet("{userId}/ips")]
        public async Task<IActionResult> GetUserIpAddresses(
            [FromQuery][Required] long userId)
        {
            _logger.LogTrace("GetUserIpAddresses started");
            var ips = await _userIpAdresesService.GetUserIpsAsync(userId);
            _logger.LogTrace("GetUserIpAddresses finished");

            return Ok(ips);
        }

        [HttpGet("{userId}/last-connection")]
        public async Task<IActionResult> GetLastConnection(
            [FromQuery][Required] long userId)
        {
            _logger.LogTrace("GetLastConnection started");
            var lastConnection = await _userService.GetLastConnectionAsync(userId);
            _logger.LogTrace("GetLastConnection finished");

            return Ok(lastConnection);
        }
    }
}
