using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using TvShowTracker.Domain.Models;
using TvShowTracker.Domain.Services;

namespace TvShowTracker.Api.Controllers
{
    public class TvShowsController : BaseController
    {
        private readonly ITvShowService _showService;
        private readonly IHashingService _hashingService;
        private readonly IMemoryCache _memoryCache;

        public TvShowsController(ITvShowService showService, IHashingService hashingService, IHttpContextAccessor httpContextAccessor,
                                 ILogger<TvShowsController> logger, IMemoryCache memoryCache) : base(httpContextAccessor, logger)
        {
            _showService = showService;
            _hashingService = hashingService;
            _memoryCache = memoryCache;
        }
        
        [HttpGet("api/v1/tv-shows")]
        public async Task<IActionResult> GetAllAsync([FromQuery] GetTvShowsFilter filter)
        {
            //this might be not the most elegant solution, but hey it works
            var cacheKey = $"tv-shows-{Convert.ToBase64String(Encoding.UTF8.GetBytes(JsonSerializer.Serialize(filter).ToLower()))}";
            var result = await _memoryCache.GetOrCreateAsync(cacheKey, async entry =>
            {
                entry.SlidingExpiration = TimeSpan.FromMinutes(1);
                return await _showService.GetAllAsync(filter);
            });
            //var result = await _showService.GetAllAsync(filter);
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

            var result = await _memoryCache.GetOrCreateAsync($"tv-shows-{id}", async entry =>
            {
                entry.SlidingExpiration = TimeSpan.FromMinutes(1);
                return await _showService.GetByIdAsync(numberId.Value);
            });

            return result.Success? Ok(result) : Problem(string.Join(",", result.Errors));
        }
    }
}
