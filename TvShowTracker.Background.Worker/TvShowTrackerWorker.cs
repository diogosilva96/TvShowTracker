using Microsoft.EntityFrameworkCore;
using TvShowTracker.DataAccessLayer;

namespace TvShowTracker.Background.Worker
{
    public class TvShowTrackerWorker : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<TvShowTrackerWorker> _logger;

        public TvShowTrackerWorker(IServiceProvider serviceProvider, ILogger<TvShowTrackerWorker> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                await Task.Delay(1000, stoppingToken);
            }
        }

    }
}