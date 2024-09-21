namespace UserIpCollector.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using UserIpCollector.Abstractions.Interfaces;
    using UserIpCollector.Services;

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

        //[HttpPost("connect")]
        //public async Task<IActionResult> AddConnection([FromBody] ConnectionRequest request)
        //{
        //    await _cacheService.AddUserIpAsync(request.UserId, request.IpAddress);
        //    await _userService.SaveConnectionToDbAsync(request.UserId, request.IpAddress);
        //    return Ok();
        //}

        //[HttpGet("find-by-ip")]
        //public async Task<IActionResult> FindUsersByIpPrefix([FromQuery] string ipPrefix)
        //{
        //    var users = await _cacheService.FindUsersByIpPrefixAsync(ipPrefix);
        //    return Ok(users);
        //}

        //[HttpGet("{userId}/ips")]
        //public async Task<IActionResult> GetUserIpAddresses(long userId)
        //{
        //    var ips = await _cacheService.GetUserIpsAsync(userId);
        //    return Ok(ips);
        //}

        //[HttpGet("{userId}/last-connection")]
        //public async Task<IActionResult> GetLastConnection(long userId)
        //{
        //    var lastConnection = await _userService.GetLastConnectionAsync(userId);
        //    return Ok(lastConnection);
        //}
    }
}
