using Microsoft.AspNetCore.Mvc;
using TvShowTracker.Domain.Models;
using TvShowTracker.Domain.Services;

namespace TvShowTracker.Api.Controllers
{
    public class TvShowsController : BaseController
    {
        private readonly ITvShowService _showService;

        public TvShowsController(ITvShowService showService, IHttpContextAccessor httpContextAccessor,
                                 ILogger<TvShowsController> logger) : base(httpContextAccessor, logger)
        {
            _showService = showService;
        }
        
        [HttpGet("api/v1/tv-shows")]
        public async Task<IActionResult> GetAllAsync([FromQuery] GetTvShowsFilter filter)
        {
            var result = await _showService.GetAllAsync(filter);
            return Ok(result);
        }
    }
}
