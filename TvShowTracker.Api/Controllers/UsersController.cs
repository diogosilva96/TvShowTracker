using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TvShowTracker.Domain.Models;
using TvShowTracker.Domain.Services;
using TvShowTracker.Infrastructure.Helpers;
using TvShowTracker.Infrastructure.Utilities;

namespace TvShowTracker.Api.Controllers
{
    public class UsersController : BaseController
    {
        private readonly IUserService _userService;
        private readonly IAuthenticationService _authenticationService;
        private readonly IHashingService _hashingService;
        private readonly ILogger<UsersController> _logger;

        public UsersController(IHttpContextAccessor httpContextAccessor, IUserService userService, IAuthenticationService authenticationService, IHashingService hashingService, ILogger<UsersController> logger) : base(httpContextAccessor)
        {
            _userService = userService;
            _authenticationService = authenticationService;
            _hashingService = hashingService;
            _logger = logger;
        }

        [Authorize(Roles = UserRoles.Administrator)]
        [HttpGet("api/v1/[controller]")]
        public async Task<Result<IEnumerable<UserModel>>> GetAllAsync(int page = 0, int size = 50)
        {
            var userInfo = GetAuthenticatedUserInfo();
            return await _userService.GetAllAsync(userInfo.Id,page, size);
        }

        [Authorize(Roles = UserRoles.User)]
        [Authorize]
        [HttpGet("api/v1/[controller]/{id}")]
        public async Task<IActionResult> GetByIdAsync(string id)
        {
            
            var decodedId = _hashingService.Decode(id);
            if (decodedId is null)
            {
                return NotFound(decodedId);
            }

            var userInfo = GetAuthenticatedUserInfo();
            var result = await _userService.GetByIdAsync(decodedId.Value,userInfo.Id);
            return result.Success ? Ok(result) : NotFound(result.Errors);
        }

        [AllowAnonymous]
        [HttpPost("api/v1/[controller]/register")]
        public async Task<IActionResult> RegisterAsync(RegisterUserModel registerUser)
        {
            var createResult = await _userService.RegisterAsync(registerUser);
            return !createResult.Success ? Problem(string.Join(",",createResult.Errors)) : Ok(createResult);
        }

        [AllowAnonymous]
        [HttpPost("api/v1/[controller]/authenticate")]
        public async Task<IActionResult> AuthenticateAsync(LoginModel loginModel)
        {
            var authResult = await _authenticationService.AuthenticateAsync(loginModel);
            if (!authResult.AuthenticationSuccess)
            {
                return Unauthorized(authResult.ErrorMessage);
            }
            return Ok(authResult);
        }
    }
}