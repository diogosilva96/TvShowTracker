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

        [HttpGet("api/v1/[controller]"), Authorize(Roles = UserRoles.Administrator)]
        public async Task<IActionResult> GetAllAsync([FromQuery]GetUsersFilter filter)
        {
            var userInfo = GetAuthenticatedUserInfo();
            return Ok(await _userService.GetAllAsync(userInfo.Id, filter));
        }
        [HttpGet("api/v1/[controller]/csv"),Authorize(Roles = UserRoles.Administrator)]
        public async Task<IActionResult> GetAllToCsvAsync([FromQuery] GetUsersFilter filter)
        {
            var userInfo = GetAuthenticatedUserInfo();
            var result = await _userService.GetAllAsync(userInfo.Id, filter);
            if (!result.Success)
            {
                return Ok(result);
            }
            return File(CsvConverter.GetCsvBytes(result.Data), "text/csv", $"users_{DateTime.Now.ToString("yyyyMMddHHmmss")}.csv");
        }

        [HttpGet("api/v1/[controller]/{id}"), Authorize(Roles = UserRoles.User)]
        public async Task<IActionResult> GetByIdAsync(string id)
        {
            var decodedId = _hashingService.Decode(id);
            if (decodedId is null)
            {
                return NotFound(id);
            }

            var userInfo = GetAuthenticatedUserInfo();
            var result = await _userService.GetByIdAsync(decodedId.Value,userInfo.Id);
            return result.Success ? Ok(result) : NotFound(result.Errors);
        }

        [HttpGet("api/v1/[controller]/{id}/favoriteShows"), Authorize(Roles = UserRoles.User)]
        public async Task<IActionResult> GetFavoriteShowsAsync(string id)
        {
            var decodedId = _hashingService.Decode(id);
            if (decodedId is null)
            {
                return NotFound(id);
            }

            var userInfo = GetAuthenticatedUserInfo();
            var result = await _userService.GetFavoriteShowsAsync(decodedId.Value, userInfo.Id);
            return result.Success ? Ok(result) : NotFound(result.Errors);
        }

        [HttpDelete("api/v1/[controller]/{id}/delete"), Authorize(Roles = UserRoles.User)]
        public async Task<IActionResult> DeleteAsync(string id)
        {
            var decodedId = _hashingService.Decode(id);
            if (decodedId is null)
            {
                return NotFound(id);
            }

            var userInfo = GetAuthenticatedUserInfo();
            var result = await _userService.DeactivateAsync(decodedId.Value, userInfo.Id);
            return result.Success ? Ok(result) : NotFound(result.Errors);
        }
      
        [HttpPost("api/v1/[controller]/register"), AllowAnonymous]
        public async Task<IActionResult> RegisterAsync(RegisterUserModel registerUser)
        {
            var createResult = await _userService.RegisterAsync(registerUser);
            return !createResult.Success ? Problem(string.Join(",",createResult.Errors)) : Ok(createResult);
        }

        
        [HttpPost("api/v1/[controller]/authenticate"), AllowAnonymous]
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