using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using TvShowTracker.Background.Worker.Services;
using TvShowTracker.DataAccessLayer;

namespace TvShowTracker.Background.Worker
{
    public class TvShowTrackerWorker : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<TvShowTrackerWorker> _logger;

        public TvShowTrackerWorker(IServiceProvider serviceProvider, IConfiguration configuration, ILogger<TvShowTrackerWorker> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
            var syncDelay = int.TryParse(configuration["SynchronizationDelayInMinutes"], out var delayInMinutes) ? delayInMinutes : 10;
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Stopping.");
            return base.StopAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            //wait 15 seconds before starting
            var initialWait = new TimeSpan(0, 0, 15);
            
            _logger.LogInformation("Waiting for {time} seconds before starting...", initialWait.Seconds);
            await Task.Delay(initialWait, stoppingToken);
            var watch = new Stopwatch();
            Stopwatch.StartNew();

            using (var scope = _serviceProvider.CreateScope())
            {
                var synchronizationService =
                    scope.ServiceProvider.GetRequiredService<ISynchronizationService>();
                await synchronizationService.ExecuteAsync();
            }
            watch.Stop();
            _logger.LogInformation("Synchronization completed after {time} seconds...", watch.Elapsed.Seconds);
            
        }

    }
}