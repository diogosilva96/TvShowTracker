using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using TvShowTracker.Domain.Models;
using TvShowTracker.Domain.Services;
using TvShowTracker.Infrastructure.Utilities;

namespace TvShowTracker.Api.Controllers
{
    public class UsersController : BaseController
    {
        private readonly IMemoryCache _memoryCache;
        private readonly IUserService _userService;
        private readonly IAuthenticationService _authenticationService;
        private readonly IHashingService _hashingService;

        public UsersController(IHttpContextAccessor httpContextAccessor, IMemoryCache memoryCache, IUserService userService, IAuthenticationService authenticationService, IHashingService hashingService, ILogger<UsersController> logger) : base(httpContextAccessor, logger)
        {
            _memoryCache = memoryCache;
            _userService = userService;
            _authenticationService = authenticationService;
            _hashingService = hashingService;
        }

        [HttpGet("api/v1/[controller]"), Authorize(Roles = UserRoles.Administrator)]
        public async Task<IActionResult> GetAllAsync([FromQuery]GetUsersFilter filter)
        {
            var userInfo = GetAuthenticatedUserInfo();
            //this might be not the most elegant solution, but hey it works
            var cacheKey = $"users-get-all-{Convert.ToBase64String(Encoding.UTF8.GetBytes(JsonSerializer.Serialize(filter).ToLower()))}";
            var result = await _memoryCache.GetOrCreateAsync(cacheKey, async entry =>
            {
                entry.SlidingExpiration = TimeSpan.FromMinutes(1);
                var result = await _userService.GetAllAsync(userInfo.Id,filter);
                return result.Success ? Ok(result) : Problem(string.Join(",", result.Errors));
            });
            return result;
        }

        [HttpGet("api/v1/files/csv/[controller]"),Authorize(Roles = UserRoles.Administrator)]
        public async Task<IActionResult> GetAllToCsvAsync([FromQuery] GetUsersFilter filter)
        {
            var userInfo = GetAuthenticatedUserInfo();
            var result = await _userService.GetAllAsync(userInfo.Id, filter);
            if (!result.Success)
            {
                return Problem(string.Join(",", result.Errors));
            }
            return File(CsvConverter.GetCsvBytes(result.Data), "text/csv", $"users_{DateTime.Now.ToString("yyyyMMddHHmmss")}.csv");
        }

        [HttpGet("api/v1/[controller]/{id}"), Authorize(Roles = UserRoles.UserOrAdministrator)]
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

        [HttpGet("api/v1/[controller]/{id}/favorite-shows"), Authorize(Roles = UserRoles.UserOrAdministrator)]
        public async Task<IActionResult> GetFavoriteShowsAsync(string id)
        {
            var decodedId = _hashingService.Decode(id);
            if (decodedId is null)
            {
                return NotFound(id);
            }

            var userInfo = GetAuthenticatedUserInfo();
            var result = await _userService.GetFavoriteShowsAsync(decodedId.Value, userInfo.Id);
            return result.Success ? Ok(result) : Problem(string.Join(",", result.Errors));
        }

        [HttpPost("api/v1/[controller]/{id}/favorite-shows"), Authorize(Roles = UserRoles.UserOrAdministrator)]
        public async Task<IActionResult> AddFavoriteShowsAsync(string id, [FromBody] FavoriteShowRequest request)
        {
            if (!request.ShowIds.Any())
            {
                return Problem("Empty shows array.");
            }
            var decodedId = _hashingService.Decode(id);
            var decodedShowIds = request.ShowIds.Select(sId => _hashingService.Decode(sId))
                                                                                      .Where(s=>s.HasValue)
                                                                                      .Cast<int>()
                                                                                      .ToList();
            if (decodedId is null || !decodedShowIds.Any())
            {
                return decodedId is null ? NotFound(id) : NotFound(request.ShowIds);
            }
            var userInfo = GetAuthenticatedUserInfo();
            var result = await _userService.AddFavoriteShowsAsync(decodedId.Value, decodedShowIds, userInfo.Id);
            return result.Success ? Ok() : Problem(string.Join(",", result.Errors));
        }

        [HttpDelete("api/v1/[controller]/{id}/favorite-shows"), Authorize(Roles = UserRoles.UserOrAdministrator)]
        public async Task<IActionResult> RemoveFavoriteShowsAsync(string id, [FromBody]FavoriteShowRequest request)
        {
            if (!request.ShowIds.Any())
            {
                return Problem("Empty shows array.");
            }
            var decodedId = _hashingService.Decode(id);
            var decodedShowIds = request.ShowIds.Select(sId => _hashingService.Decode(sId))
                                        .Where(s => s.HasValue)
                                        .Cast<int>()
                                        .ToList();
            if (decodedId is null || !decodedShowIds.Any())
            {
                return decodedId is null ? NotFound(id) : NotFound(request.ShowIds);
            }
            var userInfo = GetAuthenticatedUserInfo();
            var result = await _userService.RemoveFavoriteShowsAsync(decodedId.Value, decodedShowIds, userInfo.Id);
            return result.Success ? Ok() : Problem(string.Join(",", result.Errors));
        }

        [HttpDelete("api/v1/[controller]/{id}"), Authorize(Roles = UserRoles.UserOrAdministrator)]
        public async Task<IActionResult> DeleteAsync(string id)
        {
            var decodedId = _hashingService.Decode(id);
            if (decodedId is null)
            {
                return NotFound(id);
            }

            var userInfo = GetAuthenticatedUserInfo();
            var result = await _userService.DeactivateAsync(decodedId.Value, userInfo.Id);
            return result.Success ? Ok() : Problem(string.Join(",", result.Errors));
        }
      
        [HttpPost("api/v1/[controller]"), AllowAnonymous]
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