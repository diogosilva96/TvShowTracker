using Microsoft.EntityFrameworkCore;
using TvShowTracker.Background.Worker.Services;
using TvShowTracker.DataAccessLayer;

namespace TvShowTracker.Background.Worker
{
    public class TvShowTrackerWorker : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<TvShowTrackerWorker> _logger;
        private readonly int _pagesToSync;
        private readonly TimeSpan _syncDelay;

        public TvShowTrackerWorker(IServiceProvider serviceProvider, IConfiguration configuration, ILogger<TvShowTrackerWorker> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
            var syncDelay = int.TryParse(configuration["SynchronizationDelayInMinutes"], out var delayInMinutes) ? delayInMinutes : 10;
            _syncDelay = new TimeSpan(0, 0, syncDelay, 0);
            _pagesToSync = int.TryParse(configuration["SynchronizationPagesPerExecution"], out var pages) ? pages : 5;
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
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var synchronizationService =
                        scope.ServiceProvider.GetRequiredService<ISynchronizationService>();
                    await synchronizationService.ExecuteAsync(1,_pagesToSync);
                }
                _logger.LogInformation("Waiting for {time} minutes until new synchronization...", _syncDelay.Minutes);
                await Task.Delay(_syncDelay, stoppingToken);
            }
        }

    }
}