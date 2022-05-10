using Microsoft.AspNetCore.Mvc;
using TvShowTracker.Api.Models;
using TvShowTracker.Domain.Models;
using TvShowTracker.Domain.Services;
using TvShowTracker.Infrastructure.Helpers;

namespace TvShowTracker.Api.Controllers
{
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IHashingService _hashingService;
        private readonly ILogger<UsersController> _logger;

        public UsersController(IUserService userService, IHashingService hashingService, ILogger<UsersController> logger)
        {
            _userService = userService;
            _hashingService = hashingService;
            _logger = logger;
        }

        [HttpGet("api/v1/[controller]")]
        public async Task<Result<IEnumerable<UserDto>>> GetAllAsync(int page, int size)
        {
            return await _userService.GetAllAsync(page, size);
        }
        [HttpGet("api/v1/[controller]/{id}")]
        public async Task<IActionResult> GetByIdAsync(string id)
        {
            var decodedId = _hashingService.Decode(id);
            if (decodedId is null)
            {
                return NotFound(decodedId);
            }
       
            return Ok(await _userService.GetByIdAsync(decodedId.Value));
        }


        [HttpPost("api/v1/[controller]/register")]
        public async Task<IActionResult> RegisterAsync(RegisterUserDto registerUser)
        {
            var createResult = await _userService.RegisterAsync(registerUser);
            return !createResult.Success ? Problem(string.Join(",",createResult.Errors)) : Ok(createResult);
        }

    }
}