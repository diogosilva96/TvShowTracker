using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TvShowTracker.Domain.Models;
using TvShowTracker.Domain.Services;
using TvShowTracker.Infrastructure.Utilities;

namespace TvShowTracker.Api.Controllers
{
    public class TvShowsController : BaseController
    {
        private readonly ITvShowService _showService;
        private readonly IHashingService _hashingService;

        public TvShowsController(ITvShowService showService, IHashingService hashingService, IHttpContextAccessor httpContextAccessor,
                                 ILogger<TvShowsController> logger) : base(httpContextAccessor, logger)
        {
            _showService = showService;
            _hashingService = hashingService;
        }
        
        [HttpGet("api/v1/tv-shows")]
        public async Task<IActionResult> GetAllAsync([FromQuery] GetTvShowsFilter filter)
        {
            var result = await _showService.GetAllAsync(filter);
            return result.Success ? Ok(result) : Problem(string.Join(",", result.Errors));
        }

        [HttpGet("api/v1/tv-shows/{id}")]
        public async Task<IActionResult> GetByIdAsync(string id)
        {
            var numberId = _hashingService.Decode(id);
            if (numberId is null)
            {
                return NotFound(id);
            }
            var result = await _showService.GetByIdAsync(numberId.Value);
            return result.Success? Ok(result) : Problem(string.Join(",", result.Errors));
        }
    }
}
