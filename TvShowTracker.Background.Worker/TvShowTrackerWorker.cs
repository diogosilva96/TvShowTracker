using Microsoft.EntityFrameworkCore;
using TvShowTracker.Background.Worker.Services;
using TvShowTracker.DataAccessLayer;

namespace TvShowTracker.Background.Worker
{
    public class TvShowTrackerWorker : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IEpisodeDateApiService _episodeDateApiService;
        private readonly ILogger<TvShowTrackerWorker> _logger;

        public TvShowTrackerWorker(IServiceProvider serviceProvider, IEpisodeDateApiService episodeDateApiService, ILogger<TvShowTrackerWorker> logger)
        {
            _serviceProvider = serviceProvider;
            _episodeDateApiService = episodeDateApiService;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                await Task.Delay(50000, stoppingToken);
            }
        }

    }
}