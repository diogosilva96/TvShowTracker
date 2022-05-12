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
        protected static User User = new User();
        protected readonly IHttpContextAccessor HttpContextAccessor;
        public BaseController(IHttpContextAccessor httpContextAccessor)
        {
            HttpContextAccessor = httpContextAccessor;
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
