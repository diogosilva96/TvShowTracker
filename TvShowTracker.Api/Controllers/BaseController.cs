using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TvShowTracker.Api.Models;
using TvShowTracker.DataAccessLayer.Models;
using TvShowTracker.Domain.Models;

namespace TvShowTracker.Api.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class BaseController : Controller
    {
        protected readonly IHttpContextAccessor HttpContextAccessor;
        protected readonly ILogger<BaseController> Logger;
        public BaseController(IHttpContextAccessor httpContextAccessor, ILogger<BaseController> logger)
        {
            HttpContextAccessor = httpContextAccessor;
            Logger = logger;
        }

        protected AuthenticatedUserInfo? GetAuthenticatedUserInfo()
        {
            if (HttpContextAccessor.HttpContext is null)
            {
                return null;
            }
            return new()
            {
                Id =    int.Parse(HttpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Name)),
                Email = HttpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Email)
            };
        }
    }
}
